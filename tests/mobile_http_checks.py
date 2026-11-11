import os,json,urllib.request,urllib.error,http.cookiejar
from pathlib import Path
base=os.environ.get("CAFESTOCK_MOBILE_URL","http://127.0.0.1:5186")
checks=[]
class Client:
 def __init__(self):
  self.cookies=http.cookiejar.CookieJar();self.opener=urllib.request.build_opener(urllib.request.HTTPCookieProcessor(self.cookies));self.csrf=""
 def call(self,path,body=None,expected=200,csrf=True):
  headers={}
  if body is not None:headers={"Content-Type":"application/json",**({"X-CSRF-TOKEN":self.csrf} if csrf else {})}
  req=urllib.request.Request(base+"/api/"+path,data=json.dumps(body).encode() if body is not None else None,headers=headers)
  try:
   response=self.opener.open(req,timeout=15);code=response.status;raw=response.read()
  except urllib.error.HTTPError as e:code=e.code;raw=e.read()
  value=json.loads(raw) if raw else {}
  assert code==expected,(path,code,value)
  return value
 def login(self,user,password="123456"):
  self.csrf=self.call("session")["csrf"];self.call("login",{"username":user,"password":password});session=self.call("session");self.csrf=session["csrf"];return session
def ok(v,msg):
 assert v,msg
 checks.append(msg);print(msg,flush=True)
try:
 person=Client();initial=person.call("session");assert initial.get("demo") is True,"Test yalnız Mobil Demo ortamına uygulanır";person.csrf=initial["csrf"]
 person.call("login",{"username":"Demo Personel","password":"123456"},400,False);ok(True,"CSRF olmadan giriş reddedildi")
 person.call("login",{"username":"Demo Personel","password":"yanlis"},401);ok(True,"Hatalı parola reddedildi")
 ps=person.login("Demo Personel");ws=person.call("workspace")
 ok(not ps["person"]["manage"] and not ws["staff"] and not ws["analysis"],"Personel yönetici verilerini görmüyor")
 person.call("products",{"id":None,"name":"Yetkisiz","category":"TEST","unit":"Adet","minimumStock":1},403);ok(True,"Personel ürün ekleyemiyor")
 ticket=person.call("counts/begin",{"area":"Bar"});person.call("counts/submit",{"ticket":ticket["ticket"],"lines":[]},409)
 lines=[{"productId":p["id"],"bar":p["bar"]+(1 if i==0 else 0),"depot":None} for i,p in enumerate(ws["stocks"])]
 body={"ticket":ticket["ticket"],"lines":lines};sent=person.call("counts/submit",body);retry=person.call("counts/submit",body)
 pending=person.call("workspace");ok(sent["id"]==retry["id"] and len(pending["counts"])==1,"Tekrarlanan gönderim aynı sayımı döndürüyor")
 ok(pending["stocks"]==ws["stocks"],"Mobil sayım onay öncesinde stok değiştirmiyor")
 person.call("counts/review",{"id":sent["id"],"approved":True,"note":""},403);ok(True,"Personel kendine sayım onayı veremiyor")
 manager=Client();manager.login("Demo Müdür");manager.call("counts/review",{"id":sent["id"],"approved":True,"note":"Kontrol edildi"})
 after=manager.call("workspace");ok(after["stocks"][0]["bar"]==ws["stocks"][0]["bar"]+1 and after["counts"][0]["status"]=="Onaylandı","Müdür onayı Bar farkını bir kez kaydediyor")
 manager.call("counts/review",{"id":sent["id"],"approved":True,"note":""},409);ok(True,"İkinci onay engellendi")
 ok(person.call("session")["person"]["id"]==ps["person"]["id"],"Eşzamanlı personel ve müdür oturumları birbirini etkilemiyor")
 owner=Client();owner_session=owner.login("Demo Sahip")
 owner.call("staff",{"id":None,"name":"Test Yeni Personel","role":"Personel","phone":"5551234567","hireDate":"2026-10-17"})
 staff=owner.call("workspace")["staff"];new=next(p for p in staff if p["name"]=="Test Yeni Personel")
 ok(new["phone"]=="0555 123 45 67" and new["enabled"],"Personel ve hesap birlikte oluşturuldu; telefon biçimlendi")
 newclient=Client();newclient.login(new["username"])
 owner.call("accounts",{"employeeId":new["id"],"enabled":False,"password":None});newclient.call("workspace",expected=401);ok(True,"Kapatılan hesap mevcut mobil oturumda da engellendi")
 owner.call("accounts",{"employeeId":owner_session["person"]["id"],"enabled":False,"password":None},409);ok(True,"Kendi yönetici hesabını kapatma engellendi")
 stale=person.call("counts/begin",{"area":"Bar"});before=owner.call("workspace");first=before["stocks"][0]
 import uuid
 movement={"id":str(uuid.uuid4()),"productId":first["id"],"area":"Bar","type":"Giriş","quantity":2,"note":"Test"}
 owner.call("movements",movement);owner.call("movements",movement);after=owner.call("workspace")
 ok(after["stocks"][0]["bar"]==before["stocks"][0]["bar"]+2,"Tekrarlanan stok hareketi çoğalmıyor")
 staleLines=[{"productId":p["id"],"bar":p["bar"],"depot":None} for p in before["stocks"]]
 person.call("counts/submit",{"ticket":stale["ticket"],"lines":staleLines},409);ok(True,"Stok değişince eski mobil taslak reddedildi")
 person.call("counts/submit",{"ticket":"bozuk-imza","lines":staleLines},409);ok(True,"İmzalı taslak değiştirilince gönderim reddedildi")
 owner.call("products",{"id":None,"name":"Mobil Test Ürünü","category":"TEST","unit":"Adet","minimumStock":2})
 temp=next(p for p in owner.call("workspace")["stocks"] if p["name"]=="Mobil Test Ürünü")
 owner.call("products/remove",{"id":temp["id"]})
 ok(not any(p["id"]==temp["id"] for p in owner.call("workspace")["stocks"]),"Yönetici ürün ekleme ve aktif listeden kaldırma")
 owner.call("settings",{"business":"Mobil Demo Kontrol"});ok(owner.call("workspace")["business"]=="Mobil Demo Kontrol","İşletme ayarı kaydedildi")
 person.call("counts/begin",{"area":"Both"},409)
 bar_ticket=person.call("counts/begin",{"area":"Bar"});depot_ticket=person.call("counts/begin",{"area":"Depo"})
 areas_before=owner.call("workspace")
 bar_lines=[{"productId":p["id"],"bar":p["bar"]+1,"depot":None} for p in areas_before["stocks"]]
 wrong=[dict(line) for line in bar_lines];wrong[0]["depot"]=1
 person.call("counts/submit",{"ticket":bar_ticket["ticket"],"lines":wrong},409)
 b=person.call("counts/submit",{"ticket":bar_ticket["ticket"],"lines":bar_lines})
 owner.call("counts/review",{"id":b["id"],"approved":True,"note":"Bar sayımı"})
 after_bar=owner.call("workspace")
 ok(all(p["depot"]==q["depot"] and p["bar"]==q["bar"]+1 for p,q in zip(after_bar["stocks"],areas_before["stocks"])),"Bar onayı yalnız Bar stoğunu değiştiriyor; Depo korunuyor")
 depot_lines=[{"productId":p["id"],"bar":None,"depot":p["depot"]+2} for p in areas_before["stocks"]]
 d=person.call("counts/submit",{"ticket":depot_ticket["ticket"],"lines":depot_lines})
 owner.call("counts/review",{"id":d["id"],"approved":True,"note":"Depo sayımı"})
 after_depot=owner.call("workspace")
 ok(all(p["bar"]==q["bar"] and p["depot"]==q["depot"]+2 for p,q in zip(after_depot["stocks"],after_bar["stocks"])),"Depo onayı yalnız Depo stoğunu değiştiriyor; Bar korunuyor")
 ok({c["area"] for c in after_depot["counts"]}=={"Bar","Depo"},"Alanlar ayrı kayıtlar halinde gösteriliyor")
 ok(True,"Diğer alanın değişmesi bağımsız sayımı bozmadı; yanlış alana miktar gönderimi reddedildi")
 saved_history=owner.call("workspace")
 owner.call("staff/remove",{"id":ps["person"]["id"]})
 removed=owner.call("workspace")
 ok(not any(p["id"]==ps["person"]["id"] for p in removed["staff"]) and len(removed["counts"])==len(saved_history["counts"]) and len(removed["movements"])==len(saved_history["movements"]),"Personel kaldırıldı; geçmiş sayım ve hareketler korundu")
 person.call("session",expected=401);ok(True,"Kaldırılan personelin açık oturumu kapandı")
 owner.call("staff/remove",{"id":owner_session["person"]["id"]},409);ok(True,"Yönetici kendisini personel listesinden kaldıramıyor")
 for asset in ("/","/app.js","/count-workflow.js","/phone-preview.js","/app.css","/images/bottle.png"):
  response=urllib.request.urlopen(base+asset,timeout=10);ok(response.status==200,asset+" sunuluyor")
 ok(all("hash" not in p and "salt" not in p for p in staff),"HTTP personel yanıtında parola hash/salt yok")
 checks.append("TÜM HTTP KONTROLLERİ GEÇTİ — Yalıtılmış bellek içi demo; canlı SQL testi değildir.")
except Exception as e:
 checks.append("BAŞARISIZ: "+repr(e));raise
finally:
 if os.environ.get("CAFESTOCK_TEST_RESULT"):Path(os.environ["CAFESTOCK_TEST_RESULT"]).write_text("\n".join(checks),encoding="utf-8")

