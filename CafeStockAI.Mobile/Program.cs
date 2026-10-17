using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.RateLimiting;
using CafeStockAI.Application;
using CafeStockAI.Application.Models;
using CafeStockAI.Application.Mobile;
using CafeStockAI.Application.Analysis;
using CafeStockAI.Infrastructure;
using CafeStockAI.Mobile;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder=WebApplication.CreateBuilder(args);
bool demo=args.Contains("--demo")||builder.Configuration["CAFESTOCK_DEMO"]=="1";
builder.WebHost.ConfigureKestrel(o=>o.Limits.MaxRequestBodySize=512*1024);
var keys=builder.Configuration["CAFESTOCK_KEY_DIRECTORY"]??Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"CafeStockAI.Mobile","keys");
builder.Services.AddDataProtection().SetApplicationName("CafeStockAI.Mobile").PersistKeysToFileSystem(new DirectoryInfo(keys));
builder.Services.AddAntiforgery(o=>{o.HeaderName="X-CSRF-TOKEN";o.Cookie.SameSite=SameSiteMode.Strict;o.Cookie.SecurePolicy=builder.Environment.IsDevelopment()?CookieSecurePolicy.SameAsRequest:CookieSecurePolicy.Always;});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o=>{
 o.Cookie.Name="CafeStockAI.Mobile.Session";o.Cookie.HttpOnly=true;o.Cookie.SameSite=SameSiteMode.Strict;o.Cookie.SecurePolicy=builder.Environment.IsDevelopment()?CookieSecurePolicy.SameAsRequest:CookieSecurePolicy.Always;
 o.ExpireTimeSpan=TimeSpan.FromHours(8);o.SlidingExpiration=false;
 o.Events.OnRedirectToLogin=c=>{c.Response.StatusCode=401;return Task.CompletedTask;};o.Events.OnRedirectToAccessDenied=c=>{c.Response.StatusCode=403;return Task.CompletedTask;};
});
builder.Services.AddAuthorization();
builder.Services.AddRateLimiter(o=>{o.RejectionStatusCode=429;o.AddPolicy("login",c=>RateLimitPartition.GetFixedWindowLimiter(c.Connection.RemoteIpAddress?.ToString()??"local",_=>new(){PermitLimit=10,Window=TimeSpan.FromMinutes(1),QueueLimit=0}));});
if(demo)builder.Services.AddSingleton<IInventoryPersistence,DemoPersistence>();
else builder.Services.AddSingleton<IInventoryPersistence>(_=>{
 string? connection=builder.Configuration["CAFESTOCK_SQL_CONNECTION"];
 if(string.IsNullOrWhiteSpace(connection)){
  string folder=builder.Configuration["CAFESTOCK_DESKTOP_DIRECTORY"]??Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"CafeStockAI");
  string file=Path.Combine(folder,"connection.local.json");
  if(!File.Exists(file))return new UnavailablePersistence();
  using var json=JsonDocument.Parse(File.ReadAllText(file));
  string server=json.RootElement.GetProperty("Server").GetString()!,database=json.RootElement.GetProperty("Database").GetString()!;
  bool local=new[]{".","localhost","(local)",Environment.MachineName}.Contains(server.Split('\\',',')[0],StringComparer.OrdinalIgnoreCase);
  connection=new SqlConnectionStringBuilder{DataSource=server,InitialCatalog=database,IntegratedSecurity=true,Encrypt=true,TrustServerCertificate=local,ConnectTimeout=5,ApplicationName="CafeStockAI.Mobile"}.ConnectionString;
 }
 return new SqlInventoryPersistence(()=>DatabaseOptions.Create(connection));
});
builder.Services.AddScoped<MobileWorkspace>();
// Serializes web commands; SQL rowversion also detects writes from Desktop or other processes.
builder.Services.AddSingleton(new SemaphoreSlim(1,1));
var app=builder.Build();
app.Use(async(ctx,next)=>{
 // This development deliverable deliberately has no internet/LAN binding or tunnel.
 if(!IPAddress.IsLoopback(ctx.Connection.RemoteIpAddress??IPAddress.None)||!new[]{"localhost","127.0.0.1","::1"}.Contains(ctx.Request.Host.Host,StringComparer.OrdinalIgnoreCase)){ctx.Response.StatusCode=403;return;}
 ctx.Response.Headers["X-Content-Type-Options"]="nosniff";ctx.Response.Headers["X-Frame-Options"]="DENY";
 ctx.Response.Headers["Content-Security-Policy"]="default-src 'self'; img-src 'self' data:; style-src 'self'; script-src 'self'; connect-src 'self'; frame-ancestors 'none'; base-uri 'self'; form-action 'self'";
 ctx.Response.Headers.CacheControl="no-store";
 try{await next();}
 catch(UnauthorizedAccessException e){ctx.Response.StatusCode=403;await ctx.Response.WriteAsJsonAsync(new{error=e.Message});}
 catch(AntiforgeryValidationException){ctx.Response.StatusCode=400;await ctx.Response.WriteAsJsonAsync(new{error="Güvenlik doğrulaması yenilenmeli. Sayfayı yenileyin."});}
 catch(Exception e) when(e is InvalidOperationException or InvalidDataException or ArgumentException or JsonException or CryptographicException){ctx.Response.StatusCode=409;await ctx.Response.WriteAsJsonAsync(new{error=e is CryptographicException?"Sayım taslağı doğrulanamadı. Yeni sayım başlatın.":e.Message});}
 catch(DbUpdateException){ctx.Response.StatusCode=409;await ctx.Response.WriteAsJsonAsync(new{error="İşlem mevcut kayıtlarla çakıştı. Sayfayı yenileyip tekrar deneyin."});}
 catch(Exception e){app.Logger.LogError(e,"Mobil işlem tamamlanamadı");ctx.Response.StatusCode=503;await ctx.Response.WriteAsJsonAsync(new{error="Veritabanına ulaşılamadı. Masaüstü SQL bağlantısını ve sunucu hizmetini kontrol edin. Taslağınız korunuyor."});}
});
app.UseDefaultFiles();app.UseStaticFiles();
app.UseRouting();app.UseRateLimiter();app.UseAuthentication();app.UseAuthorization();
app.Use(async(ctx,next)=>{
 if(ctx.Request.Path.StartsWithSegments("/api")&&HttpMethods.IsPost(ctx.Request.Method))await ctx.RequestServices.GetRequiredService<IAntiforgery>().ValidateRequestAsync(ctx);
 if(ctx.User.Identity?.IsAuthenticated==true&&ctx.Request.Path.StartsWithSegments("/api")){
  var data=ctx.RequestServices.GetRequiredService<IInventoryPersistence>().Export();
  var id=Guid.Parse(ctx.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
  var account=data.Accounts.SingleOrDefault(a=>a.EmployeeId==id&&a.Enabled);
  if(account is null||AccountCredentials.Stamp(account)!=ctx.User.FindFirstValue("credential")){await ctx.SignOutAsync();ctx.Response.StatusCode=401;return;}
  MobileWorkspace.Person(data,id);
 }
 await next();
});
var commands=app.MapGroup("");
commands.AddEndpointFilter(async(context,next)=>{
 var gate=context.HttpContext.RequestServices.GetRequiredService<SemaphoreSlim>();await gate.WaitAsync(context.HttpContext.RequestAborted);
 try{return await next(context);}finally{gate.Release();}
});
Guid UserId(HttpContext c)=>Guid.Parse(c.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
app.MapGet("/images/{file}",(string file)=>{
 var assembly=typeof(DemoPersistence).Assembly;
 var resource="Mobile.Images."+file;
 if(!assembly.GetManifestResourceNames().Contains(resource))return Results.NotFound();
 return Results.File(assembly.GetManifestResourceStream(resource)!,"image/png");
});
app.MapGet("/api/session",(HttpContext c,IAntiforgery af,IInventoryPersistence store)=>{
 var csrf=af.GetAndStoreTokens(c).RequestToken;
 if(c.User.Identity?.IsAuthenticated!=true)return Results.Ok(new{authenticated=false,csrf,demo});
 var a=store.Export();var p=MobileWorkspace.Person(a,UserId(c));
 return Results.Ok(new{authenticated=true,csrf,demo,person=new{id=p.Id,p.Name,p.Role,branch="Elazığ",manage=MobileWorkspace.Global(p),approve=MobileWorkspace.Approver(p),movement=MobileWorkspace.Movement(p)},business=a.Inventory.Settings.BusinessName});
});
commands.MapPost("/api/login",async(LoginInput input,HttpContext c,IInventoryPersistence store)=>{
 if(string.IsNullOrWhiteSpace(input.Username)||input.Username.Length>160||string.IsNullOrEmpty(input.Password)||input.Password.Length>128)return Results.BadRequest(new{error="Kullanıcı adı ve şifre girin."});
 var a=store.Export();var user=a.Accounts.FirstOrDefault(u=>u.Enabled&&AccountCredentials.Key(u.Username)==AccountCredentials.Key(input.Username));
 if(user is null||!AccountCredentials.Verify(user,input.Password))return Results.Json(new{error="Kullanıcı adı veya şifre hatalı."},statusCode:401);
 var person=MobileWorkspace.Person(a,user.EmployeeId);
 await c.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(new[]{new Claim(ClaimTypes.NameIdentifier,person.Id.ToString()),new Claim("credential",AccountCredentials.Stamp(user))},CookieAuthenticationDefaults.AuthenticationScheme)));
 return Results.Ok(new{ok=true});
}).RequireRateLimiting("login");
commands.MapPost("/api/logout",async(HttpContext c)=>{await c.SignOutAsync();return Results.Ok(new{ok=true});}).RequireAuthorization();
app.MapGet("/api/workspace",(HttpContext c,MobileWorkspace service)=>{
 var a=service.Snapshot(UserId(c));var person=MobileWorkspace.Person(a,UserId(c));
 var stocks=a.Products.Select(p=>new{p.Id,p.Name,p.Category,p.Unit,p.MinimumStock,image="/images/"+ImageKey.For(p)+".png",bar=MobileWorkspace.Balance(a.Inventory,p.Id,"Bar"),depot=MobileWorkspace.Balance(a.Inventory,p.Id,"Depo")}).ToList();
 var counts=a.Inventory.Counts.Where(v=>v.BranchName=="Elazığ"&&(MobileWorkspace.Approver(person)||v.Operator==person.Name)).OrderByDescending(v=>v.Date).Take(100).Select(v=>new{v.Id,v.Date,v.Operator,v.Status,v.Reviewer,v.ReviewNote,v.ReviewedAt,lines=v.Lines.Select(l=>new{l.ProductId,name=a.Products.FirstOrDefault(p=>p.Id==l.ProductId)?.Name??"Kaldırılmış ürün",unit=a.Products.FirstOrDefault(p=>p.Id==l.ProductId)?.Unit??"",l.BarExpected,l.DepotExpected,l.BarCounted,l.DepotCounted})});
 var areas=a.Inventory.Warehouses.ToDictionary(w=>w.Id,w=>w.StockArea);
 var movements=MobileWorkspace.Movement(person)?a.Inventory.Movements.OrderByDescending(m=>m.Date).Take(100).Select(m=>new{m.Id,m.Date,m.Type,m.Quantity,m.Note,area=areas.GetValueOrDefault(m.WarehouseId,""),name=a.Products.FirstOrDefault(p=>p.Id==m.ProductId)?.Name??"Kaldırılmış ürün"}).ToArray():[];
 var analysis=MobileWorkspace.Approver(person)?a.Products.Select(p=>new{product=p,model=DemandModel.Train(a.Inventory,p.Id,"Elazığ")}).Where(x=>x.model is not null).Select(x=>new{x.product.Id,x.product.Name,x.product.Unit,sevenDays=StockPlanning.Demand(x.product.Unit,x.model!.SevenDays),mae=x.model!.Error,days=x.model!.Samples,reorder=StockPlanning.Reorder(x.product.Unit,x.model!.SevenDays,x.product.MinimumStock,MobileWorkspace.Balance(a.Inventory,x.product.Id,"Bar")+MobileWorkspace.Balance(a.Inventory,x.product.Id,"Depo"))}).ToArray():[];
 var staff=MobileWorkspace.Global(person)?a.Inventory.Employees.Where(e=>e.IsActive).Select(e=>new{e.Id,e.Name,e.Role,e.Phone,e.HireDate,enabled=a.Accounts.Any(u=>u.EmployeeId==e.Id&&u.Enabled),username=a.Accounts.FirstOrDefault(u=>u.EmployeeId==e.Id)?.Username}).ToArray():[];
 return Results.Ok(new{stocks,counts,movements,analysis,staff,roles=MobileWorkspace.Roles,business=a.Inventory.Settings.BusinessName});
}).RequireAuthorization();
commands.MapPost("/api/counts/begin",(HttpContext c,MobileWorkspace service,IDataProtectionProvider dp)=>{
 var ticket=service.Begin(UserId(c));return Results.Ok(new{ticket=dp.CreateProtector("count-ticket-v1").Protect(JsonSerializer.Serialize(ticket)),ticket.Id,ticket.Expires});
}).RequireAuthorization();
commands.MapPost("/api/counts/submit",(SubmitInput input,HttpContext c,MobileWorkspace service,IDataProtectionProvider dp)=>{
 var ticket=JsonSerializer.Deserialize<CountTicket>(dp.CreateProtector("count-ticket-v1").Unprotect(input.Ticket))??throw new InvalidDataException("Geçersiz taslak.");
 var id=service.Submit(UserId(c),ticket,input.Lines);return Results.Ok(new{id,status="Onay bekliyor"});
}).RequireAuthorization();
commands.MapPost("/api/counts/review",(ReviewInput input,HttpContext c,MobileWorkspace service)=>{service.Review(UserId(c),input.Id,input.Approved,input.Note);return Results.Ok(new{ok=true});}).RequireAuthorization();
commands.MapPost("/api/movements",(MovementEdit input,HttpContext c,MobileWorkspace service)=>{service.AddMovement(UserId(c),input);return Results.Ok(new{ok=true});}).RequireAuthorization();
commands.MapPost("/api/products",(ProductEdit input,HttpContext c,MobileWorkspace service)=>{service.SaveProduct(UserId(c),input);return Results.Ok(new{ok=true});}).RequireAuthorization();
commands.MapPost("/api/products/remove",(IdInput input,HttpContext c,MobileWorkspace service)=>{service.RemoveProduct(UserId(c),input.Id);return Results.Ok(new{ok=true});}).RequireAuthorization();
commands.MapPost("/api/staff",(StaffEdit input,HttpContext c,MobileWorkspace service)=>{service.SaveStaff(UserId(c),input);return Results.Ok(new{ok=true});}).RequireAuthorization();
commands.MapPost("/api/staff/remove",(IdInput input,HttpContext c,MobileWorkspace service)=>{service.RemoveStaff(UserId(c),input.Id);return Results.Ok(new{ok=true});}).RequireAuthorization();
commands.MapPost("/api/accounts",(AccountEdit input,HttpContext c,MobileWorkspace service)=>{service.ChangeAccount(UserId(c),input);return Results.Ok(new{ok=true});}).RequireAuthorization();
commands.MapPost("/api/settings",(SettingsInput input,HttpContext c,MobileWorkspace service)=>{service.SaveSettings(UserId(c),input.Business);return Results.Ok(new{ok=true});}).RequireAuthorization();
// Commit commands are isolated per request. No static identity, no web endpoint auto-approves a count.

app.Run();
record LoginInput(string Username,string Password);
record SubmitInput(string Ticket,List<CountEntry> Lines);
record ReviewInput(Guid Id,bool Approved,string? Note);
record IdInput(Guid Id);
record SettingsInput(string Business);

sealed class UnavailablePersistence:IInventoryPersistence
{
 private static Exception Missing()=>new InvalidOperationException("Önce masaüstünde SQL bağlantısını kurun veya Visual Studio'da Mobil Demo profilini seçin.");
 public InventoryData LoadInventory()=>throw Missing();public List<Product> LoadProducts()=>throw Missing();public List<UserAccount> LoadAccounts()=>throw Missing();public InventoryArchive Export()=>throw Missing();
 public void SaveInventory(InventoryData d)=>throw Missing();public void SaveProducts(List<Product> d)=>throw Missing();public void SaveAccounts(List<UserAccount> d)=>throw Missing();public void Restore(InventoryArchive d)=>throw Missing();
}
static class ImageKey
{
 public static string For(Product p){
 var c=p.Category.ToUpperInvariant();var n=p.Name.ToUpperInvariant();
 if(n=="SU")return "water";if(n.Contains("POMPASI"))return "pump";if(n.Contains("KAPAK"))return "lid";if(n.Contains("PİPET"))return "straw";if(n=="COLD BREW")return "bottle";
 return c switch{"BARDAK/KAPAK/PİPET"=>"cup","BARDAK/TABAK"=>"mug","SÜT ÇEŞİTLERİ"=>"milk","BİTKİ ÇAYLARI"=>"tea","ANA MALZEME"=>"coffee","TOZ VE KURUTULMUŞ"=>"powder","KASA ÖNÜ"=>"snack","BAR SOSLAR" or "ŞURUPLAR" or "COOL SERIES" or "BOOM SERIES" or "LİMONATA"=>"bottle",_=>"bag"};
 }
}




