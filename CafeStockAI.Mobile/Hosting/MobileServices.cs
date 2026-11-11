using CafeStockAI.Application;
using CafeStockAI.Application.Mobile;
using CafeStockAI.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace CafeStockAI.Mobile;
internal static class MobileServices
{
    public static void AddMobileServices(this WebApplicationBuilder builder, bool demo)
    {
        builder.WebHost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = 512 * 1024);
        var keys = builder.Configuration["CAFESTOCK_KEY_DIRECTORY"] ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CafeStockAI.Mobile", "keys");
        builder.Services.AddDataProtection().SetApplicationName("CafeStockAI.Mobile").PersistKeysToFileSystem(new DirectoryInfo(keys));
        builder.Services.AddAntiforgery(o =>
        {
            o.HeaderName = "X-CSRF-TOKEN";
            o.Cookie.SameSite = SameSiteMode.Strict;
            o.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
        });
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(o =>
        {
            o.Cookie.Name = "CafeStockAI.Mobile.Session";
            o.Cookie.HttpOnly = true;
            o.Cookie.SameSite = SameSiteMode.Strict;
            o.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
            o.ExpireTimeSpan = TimeSpan.FromHours(8);
            o.SlidingExpiration = false;
            o.Events.OnRedirectToLogin = c =>
            {
                c.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
            o.Events.OnRedirectToAccessDenied = c =>
            {
                c.Response.StatusCode = 403;
                return Task.CompletedTask;
            };
        });
        builder.Services.AddAuthorization();
        builder.Services.AddRateLimiter(o =>
        {
            o.RejectionStatusCode = 429;
            o.AddPolicy("login", c => RateLimitPartition.GetFixedWindowLimiter(c.Connection.RemoteIpAddress?.ToString() ?? "local", _ => new() { PermitLimit = 10, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
        });
        if (demo)
            builder.Services.AddSingleton<IInventoryPersistence, DemoPersistence>();
        else
            builder.Services.AddSingleton<IInventoryPersistence>(_ =>
            {
                string? connection = builder.Configuration["CAFESTOCK_SQL_CONNECTION"];
                if (string.IsNullOrWhiteSpace(connection))
                {
                    string folder = builder.Configuration["CAFESTOCK_DESKTOP_DIRECTORY"] ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CafeStockAI");
                    string file = Path.Combine(folder, "connection.local.json");
                    if (!File.Exists(file))
                        return new UnavailablePersistence();
                    connection = SqlConnectionSettings.Load(file).ConnectionString("CafeStockAI.Mobile");
                }

                return new SqlInventoryPersistence(() => DatabaseOptions.Create(connection));
            });
        builder.Services.AddScoped<MobileWorkspace>();
        // Serializes web commands; SQL rowversion also detects writes from Desktop or other processes.
        builder.Services.AddSingleton(new SemaphoreSlim(1, 1));
    }
}
