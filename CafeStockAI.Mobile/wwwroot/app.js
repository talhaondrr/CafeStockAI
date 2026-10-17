'use strict';
const $=s=>document.querySelector(s), esc=v=>String(v??'').replace(/[&<>"']/g,c=>({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
const fmt=v=>v==null?'—':Number(v).toLocaleString('tr-TR',{maximumFractionDigits:6,useGrouping:false});
let session=null,data=null,page='home',draft=null,query='',category='',saving=false,lastSent=null;
const icons={home:'⌂',count:'▤',stocks:'▦',reviews:'✓',products:'◈',staff:'♙',analysis:'↗',reports:'≡',settings:'⚙',more:'⋯'};
const labels={home:'Ana sayfa',count:'Sayım',stocks:'Stoklar',reviews:'Onaylar',products:'Ürünler',staff:'Personel',analysis:'AI analizi',reports:'Raporlar',settings:'Ayarlar',more:'Diğer'};
function toast(text){$('#toast').textContent=text;$('#toast').classList.add('visible');clearTimeout(toast.timer);toast.timer=setTimeout(()=>$('#toast').classList.remove('visible'),5000);}
async function api(path,body){
 const options={credentials:'same-origin',cache:'no-store',headers:{}};
 if(body!==undefined){options.method='POST';options.headers={'Content-Type':'application/json','X-CSRF-TOKEN':session?.csrf??''};options.body=JSON.stringify(body);}
 let res;try{res=await fetch('/api/'+path,options);}catch{throw Error('Bağlantı kurulamadı. Girdiğin miktarlar bu cihazda korunuyor.');}
 let json;try{json=await res.json();}catch{json={};}
 if(!res.ok){if(res.status===401&&path!=='login'){showLogin();try{session=await (await fetch('/api/session',{credentials:'same-origin',cache:'no-store'})).json();}catch{}throw Error('Oturum süren doldu. Yeniden giriş yap.');}throw Error(json.error??(res.status===429?'Çok fazla giriş denemesi. Bir dakika sonra tekrar dene.':'İşlem tamamlanamadı.'));}
 return json;
}
function draftKey(){return 'cafestock-draft-v1-'+(session?.demo?'demo-':'sql-')+(session?.person?.id??'');}
function saveDraft(){if(!draft)return;try{localStorage.setItem(draftKey(),JSON.stringify(draft));}catch{toast('Tarayıcı taslak kaydına izin vermiyor. Sayfayı kapatmadan sayımı gönder.');}}
function restoreDraft(){try{draft=JSON.parse(localStorage.getItem(draftKey())??'null');if(draft?.expires&&new Date(draft.expires)<new Date())draft=null;}catch{draft=null;}}
function showLogin(){session=null;data=null;$('#password').value='';$('#shell').hidden=true;$('#loginPage').hidden=false;}
function menu(){let list=['home','count','stocks'];if(session.person.approve)list.push('reviews','analysis','reports');if(session.person.manage)list.push('products','staff','settings');return list;}
function nav(){
 const list=menu(),navButton=k=>'<button class="nav-button '+(page===k?'active':'')+'" data-page="'+k+'"><span class="nav-icon">'+icons[k]+'</span>'+labels[k]+'</button>';
 $('#sideNav').innerHTML=list.map(navButton).join('');
 const bottom=['home','count','stocks',session.person.approve?'reviews':'more'];if(!bottom.includes('more'))bottom.push('more');
 $('#bottomNav').innerHTML=bottom.map(navButton).join('');
 $('#avatar')?.remove();$('#logout').textContent=session.person.name.split(' ').map(x=>x[0]).slice(0,2).join('');$('#logout').title=session.person.name+' • Çıkış yap';
}
async function load(){data=await api('workspace');}
function go(next){if(!menu().includes(next)&&!['more','sent'].includes(next))return;page=next;query='';category='';render();window.scrollTo({top:0,behavior:'instant'});}
function badge(status){const color=status==='Onaylandı'?'green':status==='Reddedildi'?'red':'amber';return '<span class="pill '+color+'">'+esc(status)+'</span>';}
function date(v){return new Date(v).toLocaleString('tr-TR',{day:'numeric',month:'short',hour:'2-digit',minute:'2-digit'});}
function header(title,subtitle,action=''){return '<div class="page-head"><div><p class="page-label">ELAZIĞ ŞUBESİ</p><h1>'+title+'</h1><p class="subtitle">'+subtitle+'</p></div>'+action+'</div>';}
function countCard(c){return '<article class="card count-card" data-count="'+esc(c.id)+'"><div class="row"><span class="small">'+date(c.date)+'</span>'+badge(c.status)+'</div><h3>Elazığ • Bar ve Depo sayımı</h3><div class="row"><span class="small">'+esc(c.operator)+'</span><span class="small">'+c.lines.length+' ürün →</span></div></article>';}
function home(){
 const pending=data.counts.filter(c=>c.status==='Onay bekliyor'),critical=data.stocks.filter(p=>p.bar+p.depot<p.minimumStock);
 const first=session.person.name.split(' ')[0];
 return '<p class="hello">Merhaba, '+esc(first)+' 👋</p><h1>Bugün her şey kontrolünde.</h1><p class="subtitle">Elazığ <span> · </span>'+esc(session.person.role)+'</p>'+
 '<div class="hero"><small>GÜNLÜK STOK KONTROLÜ</small><h2>Bir sayımla başla.</h2><p>Bar ve depoyu say, miktarları gir.<br>Gerisini birlikte takip edelim.</p><button class="primary" data-page="count">'+(draft?'Kaldığın yerden devam et':'Sayıma başla')+' <span>→</span></button></div>'+
 '<div class="stat-grid"><div class="stat"><small>Toplam ürün</small><strong>'+data.stocks.length+'</strong></div><div class="stat amber"><small>Onay bekleyen</small><strong>'+pending.length+'</strong></div><div class="stat green"><small>Kritik stok</small><strong>'+critical.length+'</strong></div></div>'+
 '<div class="section-title"><h3>Son sayımlar</h3><button class="text-button" data-page="'+(session.person.approve?'reviews':'count')+'">Tümünü gör →</button></div>'+(data.counts.slice(0,3).map(countCard).join('')||'<div class="card empty">Henüz sayım yok. İlk sayımı sen başlat.</div>')+
 '<div class="section-title"><h3>Hızlı erişim</h3></div><div class="more-grid"><button data-page="stocks"><span>▦</span>Stok sorgula</button><button data-page="'+(session.person.approve?'analysis':'count')+'"><span>'+(session.person.approve?'↗':'▤')+'</span>'+(session.person.approve?'AI stok analizi':'Sayım geçmişim')+'</button></div>';
}
function completed(){return draft?draft.products.filter(p=>validInput(draft.values[p.id]?.bar)&&validInput(draft.values[p.id]?.depot)).length:0;}
function validInput(v){return v!==undefined&&v!==''&&/^\d+(?:[.,]\d{1,6})?$/.test(String(v));}
function parsed(v){if(!validInput(v))throw Error('Miktar sıfır veya pozitif bir sayı olmalı.');return Number(String(v).replace(',','.'));}
function chips(products){return '<div class="filters"><button class="chip '+(!category?'active':'')+'" data-category="">Tümü</button>'+[...new Set(products.map(p=>p.category))].map(c=>'<button class="chip '+(category===c?'active':'')+'" data-category="'+esc(c)+'">'+esc(c)+'</button>').join('')+'</div>';}
function filtered(rows){return rows.filter(p=>(!category||p.category===category)&&(!query||p.name.toLocaleLowerCase('tr').includes(query.toLocaleLowerCase('tr'))));}
function productHeading(p){return '<div class="product-top"><img class="product-image" src="'+esc(p.image)+'" alt="" loading="lazy"><div class="product-info"><h3>'+esc(p.name)+'</h3><p>'+esc(p.category)+'</p></div><span class="pill">'+esc(p.unit)+'</span></div>';}
function countPage(){
 if(!draft)return header('Şubeni birlikte sayalım.','Bar ve Depo miktarlarını tek sayımda tamamla.')+'<div class="card"><div class="row"><span class="badge-icon">▤</span><div><h3>Elazığ şubesi</h3><p class="small">'+data.stocks.length+' ürün • Bar + Depo</p></div><span class="pill green">Hazır</span></div><hr class="divider"><p class="small">Boş miktarlar sıfır kabul edilmez. Sayımın bu cihazda taslak olarak korunur. Gönderdiğinde müdür onayını bekler.</p><button id="beginCount" class="primary full">Sayıma başla →</button></div><div class="section-title"><h3>Sayım geçmişin</h3></div>'+data.counts.map(countCard).join('');
 const done=completed(),total=draft.products.length;
 return header('Ürün miktarları','Girdiğin miktarlar otomatik taslak olarak saklanır.')+
 '<div class="draft-head"><div class="row"><strong>Elazığ • Bar + Depo</strong><span class="small" id="draftProgress">'+done+' / '+total+' tamamlandı</span></div><div class="progress"><div id="progressFill"></div></div></div>'+
 '<input id="search" class="search" placeholder="Ürün ara…" aria-label="Sayılacak ürün ara" value="'+esc(query)+'">'+chips(draft.products)+
 '<div class="product-grid">'+filtered(draft.products).map(p=>{const v=draft.values[p.id]??{};return '<article class="product-card">'+productHeading(p)+'<div class="stock-pair"><label>BAR<input data-quantity="'+p.id+'" data-area="bar" inputmode="decimal" autocomplete="off" placeholder="Miktar" aria-label="'+esc(p.name)+' Bar" value="'+esc(v.bar??'')+'"></label><label>DEPO<input data-quantity="'+p.id+'" data-area="depot" inputmode="decimal" autocomplete="off" placeholder="Miktar" aria-label="'+esc(p.name)+' Depo" value="'+esc(v.depot??'')+'"></label></div><p class="product-total" id="total-'+p.id+'">Toplam: '+(validInput(v.bar)&&validInput(v.depot)?fmt(parsed(v.bar)+parsed(v.depot)):'—')+' '+esc(p.unit)+'</p></article>';}).join('')+'</div>'+
 '<div class="draft-bar"><button id="cancelDraft" class="secondary">Taslağı bırak</button><button id="reviewDraft" class="primary">Kontrol et →</button></div>';
}
function stockPage(){
 return header('Stoklar','Bar ve Depo stoklarını birlikte gör.',session.person.movement?'<button id="newMovement" class="secondary">+ Stok hareketi</button>':'')+
 '<input id="search" class="search" placeholder="Ürün ara…" aria-label="Stok ara" value="'+esc(query)+'">'+chips(data.stocks)+'<div class="product-grid">'+filtered(data.stocks).map(p=>'<article class="product-card">'+productHeading(p)+'<div class="read-stock"><div><small>BAR</small><strong>'+fmt(p.bar)+'</strong></div><div><small>DEPO</small><strong>'+fmt(p.depot)+'</strong></div><div><small>TOPLAM</small><strong>'+fmt(p.bar+p.depot)+'</strong></div></div>'+(p.bar+p.depot<p.minimumStock?'<p class="small">● Kritik sınır: '+fmt(p.minimumStock)+' '+esc(p.unit)+'</p>':'')+'</article>').join('')+'</div>';
}
function reviews(){return header('Sayım kontrolü','Personelin gönderdiği sayımları incele.')+'<div class="filters">'+['','Onay bekliyor','Onaylandı','Reddedildi'].map(s=>'<button class="chip '+(category===s?'active':'')+'" data-category="'+s+'">'+(s||'Tümü')+'</button>').join('')+'</div>'+data.counts.filter(c=>!category||c.status===category).map(countCard).join('');}
function products(){return header('Ürün yönetimi','Kataloğu ve kritik stok sınırlarını düzenle.','<button id="newProduct" class="secondary">+ Ürün ekle</button>')+'<input id="search" class="search" placeholder="Ürün ara…" aria-label="Ürün ara" value="'+esc(query)+'">'+chips(data.stocks)+'<div class="product-grid">'+filtered(data.stocks).map(p=>'<article class="product-card">'+productHeading(p)+'<div class="row"><span class="small">Kritik stok: <b>'+fmt(p.minimumStock)+'</b> '+esc(p.unit)+'</span><button class="link-button" data-edit-product="'+p.id+'">Düzenle →</button></div></article>').join('')+'</div>';}
function staff(){return header('Ekip arkadaşların','Personel, görev ve hesap erişimini yönet.','<button id="newStaff" class="secondary">+ Personel</button>')+'<div class="list-grid">'+data.staff.map(p=>'<article class="card"><div class="row"><span class="badge-icon">'+esc(p.name.split(' ').map(n=>n[0]).slice(0,2).join(''))+'</span><span class="pill '+(p.enabled?'green':'red')+'">'+(p.enabled?'Aktif hesap':'Hesap kapalı')+'</span></div><h3>'+esc(p.name)+'</h3><p class="subtitle">'+esc(p.role)+' • Elazığ</p><p class="small">'+esc(p.phone||'Telefon eklenmemiş')+'</p><p class="small">İşe giriş: '+(p.hireDate?new Date(p.hireDate).toLocaleDateString('tr-TR'):'Belirtilmemiş')+'</p><div class="actions"><button class="secondary" data-edit-staff="'+p.id+'">Bilgiler</button><button class="secondary" data-account="'+p.id+'">Hesap ayarları</button></div></article>').join('')+'</div>';}
function analysis(){return header('Stok için bir adım önde.','Geçmiş tüketimden öğrenen yerel tahmin modeli.')+'<div class="notice">7 günlük tüketim tahmini • Son 7 gün ile MAE doğrulaması. Öneriler kesin tüketim miktarı değildir.</div><div class="list-grid">'+(data.analysis.map(p=>'<article class="card"><p class="small">'+esc(p.unit)+' • '+p.days+' günlük geçmiş</p><h3>'+esc(p.name)+'</h3><hr class="divider"><div class="row"><span class="small">7 günlük tahmin</span><strong class="amount">'+fmt(p.sevenDays)+'</strong></div><div class="row"><span class="small">Sipariş önerisi</span><strong>'+fmt(p.reorder)+' '+esc(p.unit)+'</strong></div><p class="small">MAE: '+fmt(p.mae)+'</p></article>').join('')||'<div class="card empty">Tahmin için en az 14 günlük çıkış geçmişi gerekiyor.</div>')+'</div>';}
function reports(){return header('Stok raporları','Son 100 hareket ve güncel stok özeti.','<button id="exportCsv" class="secondary">CSV indir</button>')+'<div class="card scroll-table"><table><thead><tr><th>Tarih</th><th>Ürün</th><th>Alan</th><th>İşlem</th><th>Miktar</th></tr></thead><tbody>'+data.movements.map(m=>'<tr><td>'+date(m.date)+'</td><td>'+esc(m.name)+'</td><td>'+esc(m.area)+'</td><td>'+esc(m.type)+'</td><td>'+fmt(m.quantity)+'</td></tr>').join('')+'</tbody></table></div>';}
function settings(){return header('İşletme ayarları','Hesabın ve veri bağlantısı.')+'<form id="settingsForm" class="card"><label>İşletme adı<input name="business" required maxlength="160" value="'+esc(data.business)+'"></label><button class="primary">Ayarları kaydet</button></form><div class="card"><h3>'+esc(session.person.name)+'</h3><p class="subtitle">'+esc(session.person.role)+'</p><hr class="divider"><p class="small"><span class="online-dot"></span>'+(session.demo?'Yalıtılmış demo • Gerçek kayıtlara bağlı değil':'Ortak SQL veritabanı • Masaüstüyle aynı kayıtlar')+'</p><p class="small">Bu sürüm yalnız bilgisayarda yerel olarak çalışır. Mobil veri erişimi henüz açılmadı.</p><button id="refreshData" class="secondary">Verileri yenile</button></div>';}
function more(){return header('Diğer işlemler','Görevine göre kullanabileceğin ekranlar.')+'<div class="more-grid">'+menu().filter(k=>!['home','count','stocks','reviews'].includes(k)).map(k=>'<button data-page="'+k+'"><span>'+icons[k]+'</span>'+labels[k]+'</button>').join('')+'</div><div class="card"><p class="small">'+esc(session.person.name)+' • '+esc(session.person.role)+'</p><button id="mobileLogout" class="link-button">Hesaptan çıkış yap</button></div>';}
function sent(){return '<div class="success"><div class="check">✓</div><p class="page-label">ELAZIĞ ŞUBESİ</p><h2>Sayımın gönderildi.</h2><p class="small">Miktarlar kaydedildi ve müdür onayına gönderildi.<br>Onay tamamlandığında stoklar güncellenecek.</p><div class="card"><p class="small">Sayım numarası</p><strong>#'+esc(lastSent?.slice(0,8).toUpperCase())+'</strong><p>'+badge('Onay bekliyor')+'</p></div><button class="primary full" data-page="home">Ana sayfaya dön →</button></div>';}
function render(){if(!data)return;nav();const functions={home,count:countPage,stocks:stockPage,reviews,products,staff,analysis,reports,settings,more,sent};$('#main').innerHTML=(functions[page]??home)();if(draft&&$('#progressFill'))$('#progressFill').style.width=completed()/draft.products.length*100+'%';}
function modal(title,html){$('#modalTitle').textContent=title;$('#modalBody').innerHTML=html;$('#modal').showModal();}
function options(rows,value){return rows.map(r=>'<option '+(r===value?'selected':'')+' value="'+esc(r)+'">'+esc(r)+'</option>').join('');}
function productEditor(id){
 const p=data.stocks.find(p=>p.id===id)??{};
 modal(id?'Ürünü düzenle':'Yeni ürün','<form id="productForm" data-id="'+esc(id??'')+'"><label>Ürün adı<input name="name" required maxlength="160" value="'+esc(p.name??'')+'"></label><label>Kategori<input name="category" required list="categories" maxlength="160" value="'+esc(p.category??'')+'"><datalist id="categories">'+[...new Set(data.stocks.map(p=>p.category))].map(c=>'<option value="'+esc(c)+'"></option>').join('')+'</datalist></label><div class="two-fields"><label>Birim<select name="unit">'+options(['Adet','Şişe','Koli','Kg','Fişek','Paket','Litre'],p.unit)+'</select></label><label>Kritik stok<input name="minimumStock" inputmode="decimal" required value="'+esc(p.minimumStock??0)+'"></label></div><div class="error form-error"></div><button class="primary full">Kaydet</button>'+(id?'<button type="button" class="danger full" data-remove-product="'+id+'">Ürünü aktif listeden kaldır</button>':'')+'</form>');
}
function staffEditor(id){
 const p=data.staff.find(p=>p.id===id)??{};
 modal(id?'Personel bilgileri':'Personel ekle','<form id="staffForm" data-id="'+esc(id??'')+'"><label>Ad soyad<input name="name" required maxlength="160" value="'+esc(p.name??'')+'"></label><label>Görev<select name="role">'+options(data.roles,p.role??'Personel')+'</select></label><label>Telefon<input name="phone" type="tel" value="'+esc(p.phone??'')+'" placeholder="05xx xxx xx xx"></label><label>İşe giriş tarihi<input name="hireDate" type="date" value="'+esc(p.hireDate?.slice(0,10)??'')+'"></label><p class="small">Şube: Elazığ. Yeni hesap kullanıcı adı personelin adı, başlangıç şifresi 123456 olur.</p><div class="error form-error"></div><button class="primary full">Kaydet</button>'+(id&&id!==session.person.id?'<button type="button" class="danger full" data-remove-staff="'+id+'">Personeli aktif listeden kaldır</button>':'')+'</form>');
}
function accountEditor(id){
 const p=data.staff.find(p=>p.id===id);
 modal('Hesap ayarları','<form id="accountForm" data-id="'+id+'"><h3>'+esc(p.name)+'</h3><p class="subtitle">Kullanıcı adı: '+esc(p.username)+'</p><label>Hesap durumu<select name="enabled">'+options(['Açık','Kapalı'],p.enabled?'Açık':'Kapalı')+'</select></label><label>Yeni şifre<input name="password" type="password" autocomplete="new-password" minlength="6" maxlength="128" placeholder="Değişmeyecekse boş bırak"></label><div class="error form-error"></div><button class="primary full">Hesabı güncelle</button></form>');
}
function movementEditor(){modal('Stok hareketi','<form id="movementForm" data-id="'+crypto.randomUUID()+'"><label>Ürün<select name="productId">'+data.stocks.map(p=>'<option value="'+p.id+'">'+esc(p.name)+' · '+esc(p.unit)+'</option>').join('')+'</select></label><div class="two-fields"><label>Alan<select name="area">'+options(['Bar','Depo'])+'</select></label><label>İşlem<select name="type">'+options(['Giriş','Çıkış'])+'</select></label></div><label>Miktar<input name="quantity" inputmode="decimal" placeholder="0" required></label><label>Açıklama<textarea name="note" maxlength="1600"></textarea></label><div class="error form-error"></div><button class="primary full">Hareketi kaydet</button></form>');}
function countDetails(id){
 const c=data.counts.find(c=>c.id===id);if(!c)return;
 modal('Sayım detayı','<div class="row"><span class="small">'+esc(c.operator)+'</span>'+badge(c.status)+'</div><p class="small">'+date(c.date)+'</p><div class="scroll-table"><table><thead><tr><th>Ürün</th><th>Bar</th><th>Depo</th><th>Fark</th></tr></thead><tbody>'+c.lines.map(l=>'<tr><td>'+esc(l.name)+'</td><td>'+fmt(l.barCounted)+'</td><td>'+fmt(l.depotCounted)+'</td><td>'+fmt((l.barCounted??0)+(l.depotCounted??0)-(l.barExpected??0)-(l.depotExpected??0))+'</td></tr>').join('')+'</tbody></table></div>'+(c.reviewNote?'<p class="subtitle">Yönetici notu: '+esc(c.reviewNote)+'</p>':'')+(session.person.approve&&c.status==='Onay bekliyor'?'<label>Yönetici notu<textarea id="reviewNote" maxlength="2000" placeholder="Ret durumunda gerekçe zorunlu"></textarea></label><div class="actions"><button class="danger" data-review="'+id+'" data-approved="false">Reddet</button><button class="primary" data-review="'+id+'" data-approved="true">Sayımı onayla ✓</button></div><div class="error form-error"></div>':''));
}
function previewDraft(){
 const done=completed();if(done!==draft.products.length){toast((draft.products.length-done)+' ürünün Bar veya Depo miktarı eksik.');return;}
 modal('Göndermeden önce','<div class="card"><h3>Elazığ • Bar ve Depo</h3><p class="subtitle">'+done+' ürünün miktarı tamamlandı.</p><p class="small">Sayımını '+esc(session.person.name)+' adına göndereceğiz. Müdür onayına kadar stok değişmeyecek.</p></div><p class="small">Miktarları kategori filtreleriyle kontrol edebilirsin. Bağlantı hatasında tekrar gönderme aynı sayımı çoğaltmaz.</p><div class="error form-error"></div><button id="sendCount" class="primary full">Sayımı onaya gönder →</button>');
}
async function logout(){await api('logout',{});draft=null;const s=await api('session');session=s;showLogin();session=s;$('#password').value='';}
async function run(action,button){if(saving)return;saving=true;if(button)button.disabled=true;try{await action();}catch(e){const target=$('#modal[open] .form-error');if(target)target.textContent=e.message;else toast(e.message);}finally{saving=false;if(button)button.disabled=false;}}
document.addEventListener('click',event=>{
 const b=event.target.closest('button,a,[data-count]');if(!b)return;
 if(b.dataset.page){event.preventDefault();go(b.dataset.page);return;}
 if(b.dataset.category!==undefined){category=b.dataset.category;render();return;}
 if(b.dataset.count){countDetails(b.dataset.count);return;}
 if(b.dataset.editProduct){productEditor(b.dataset.editProduct);return;}
 if(b.dataset.editStaff){staffEditor(b.dataset.editStaff);return;}
 if(b.dataset.removeStaff){if(confirm('Personel aktif listeden kaldırılsın ve hesabı kapatılsın mı? İşlem geçmişi korunur.'))run(async()=>{await api('staff/remove',{id:b.dataset.removeStaff});$('#modal').close();await load();render();toast('Personel aktif listeden kaldırıldı ve hesabı kapatıldı.');},b);return;}
 if(b.dataset.account){accountEditor(b.dataset.account);return;}
 if(b.dataset.removeProduct){if(confirm('Ürün aktif listeden kaldırılsın mı? Eski hareketler korunur.'))run(async()=>{await api('products/remove',{id:b.dataset.removeProduct});$('#modal').close();await load();render();toast('Ürün aktif listeden kaldırıldı.');},b);return;}
 if(b.dataset.review){run(async()=>{await api('counts/review',{id:b.dataset.review,approved:b.dataset.approved==='true',note:$('#reviewNote').value});$('#modal').close();await load();render();toast('Sayım sonuçlandırıldı.');},b);return;}
 switch(b.id){
 case 'brandHome':event.preventDefault();go('home');break;
 case 'showPassword':$('#password').type=$('#password').type==='password'?'text':'password';b.textContent=$('#password').type==='password'?'Göster':'Gizle';break;
 case 'logout':case 'mobileLogout':run(logout,b);break;
 case 'closeModal':$('#modal').close();break;
 case 'beginCount':run(async()=>{const t=await api('counts/begin',{});draft={...t,products:structuredClone(data.stocks),values:{}};saveDraft();render();},b);break;
 case 'cancelDraft':if(confirm('Bu taslağı silip sayımdan çıkmak istiyor musun?')){localStorage.removeItem(draftKey());draft=null;render();}break;
 case 'reviewDraft':previewDraft();break;
 case 'sendCount':run(async()=>{const result=await api('counts/submit',{ticket:draft.ticket,lines:draft.products.map(p=>({productId:p.id,bar:parsed(draft.values[p.id].bar),depot:parsed(draft.values[p.id].depot)}))});lastSent=result.id;localStorage.removeItem(draftKey());draft=null;$('#modal').close();await load();go('sent');},b);break;
 case 'newProduct':productEditor();break;case 'newStaff':staffEditor();break;case 'newMovement':movementEditor();break;
 case 'refreshData':run(async()=>{await load();render();toast('Kayıtlar yenilendi.');},b);break;
 case 'exportCsv':{const cell=v=>'"'+String(v??'').replace(/"/g,'""')+'"';const csv='\uFEFF'+[['Ürün','Kategori','Birim','Bar','Depo','Toplam'],...data.stocks.map(p=>[p.name,p.category,p.unit,fmt(p.bar),fmt(p.depot),fmt(p.bar+p.depot)])].map(r=>r.map(cell).join(';')).join('\r\n');const url=URL.createObjectURL(new Blob([csv],{type:'text/csv;charset=utf-8;'}));const a=document.createElement('a');a.href=url;a.download='Elazig-Stok-Raporu.csv';a.click();URL.revokeObjectURL(url);break;}
 }
});
document.addEventListener('input',event=>{
 const input=event.target;if(input.id==='search'){query=input.value;const pos=input.selectionStart;render();$('#search').focus();$('#search').setSelectionRange(pos,pos);return;}
 if(input.dataset.quantity&&draft){const id=input.dataset.quantity;draft.values[id]??={};draft.values[id][input.dataset.area]=input.value;saveDraft();const v=draft.values[id],p=draft.products.find(p=>p.id===id);$('#total-'+id).textContent='Toplam: '+(validInput(v.bar)&&validInput(v.depot)?fmt(parsed(v.bar)+parsed(v.depot)):'—')+' '+p.unit;$('#draftProgress').textContent=completed()+' / '+draft.products.length+' tamamlandı';$('#progressFill').style.width=completed()/draft.products.length*100+'%';}
});
document.addEventListener('submit',event=>{
 const form=event.target;event.preventDefault();const fields=Object.fromEntries(new FormData(form));const b=form.querySelector('button[type="submit"],button.primary');
 if(form.id==='loginForm'){run(async()=>{try{await api('login',{username:$('#username').value,password:$('#password').value});session=await api('session');await load();restoreDraft();$('#loginPage').hidden=true;$('#shell').hidden=false;$('#modeBanner').hidden=!session.demo;go('home');}catch(e){$('#loginError').textContent=e.message;}},b);return;}
 run(async()=>{
 let endpoint,body;
 if(form.id==='productForm'){endpoint='products';body={id:form.dataset.id||null,...fields,minimumStock:parsed(fields.minimumStock)};}
 if(form.id==='staffForm'){endpoint='staff';body={id:form.dataset.id||null,...fields,hireDate:fields.hireDate||null};}
 if(form.id==='accountForm'){endpoint='accounts';body={employeeId:form.dataset.id,enabled:fields.enabled==='Açık',password:fields.password||null};}
 if(form.id==='movementForm'){endpoint='movements';body={id:form.dataset.id,...fields,quantity:parsed(fields.quantity)};}
 if(form.id==='settingsForm'){endpoint='settings';body=fields;}
 if(!endpoint)return;await api(endpoint,body);$('#modal').close();await load();render();toast('Kaydedildi.');
 },b);
});
window.addEventListener('online',()=>toast('Bağlantı geri geldi. Sayım taslağın korunuyor.'));
window.addEventListener('offline',()=>toast('Bağlantı kesildi. Miktar girmeye devam edebilirsin; göndermek için bağlantı gerekiyor.'));
async function boot(){try{session=await api('session');$('#loginMode').textContent=session.demo?'Demo hesapları: Demo Personel / Demo Müdür / Demo Sahip · Şifre: 123456':'Mevcut personel hesabınla giriş yap.';if(session.authenticated){await load();restoreDraft();$('#loginPage').hidden=true;$('#shell').hidden=false;$('#modeBanner').hidden=!session.demo;render();}}catch(e){$('#loginError').textContent=e.message;}}
boot();


