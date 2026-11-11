using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using CafeStockAI.Application;
using CafeStockAI.Application.Mobile;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace CafeStockAI.Mobile;
internal static class MobilePipeline
{
    public static void UseMobilePipeline(this WebApplication app)
    {
        app.Use(async (ctx, next) =>
        {
            // This development deliverable deliberately has no internet/LAN binding or tunnel.
            if (!IPAddress.IsLoopback(ctx.Connection.RemoteIpAddress ?? IPAddress.None) || !new[]
            {
                "localhost",
                "127.0.0.1",
                "::1"
            }.Contains(ctx.Request.Host.Host, StringComparer.OrdinalIgnoreCase))
            {
                ctx.Response.StatusCode = 403;
                return;
            }

            ctx.Response.Headers["X-Content-Type-Options"] = "nosniff";
            ctx.Response.Headers["X-Frame-Options"] = "DENY";
            ctx.Response.Headers["Content-Security-Policy"] = "default-src 'self'; img-src 'self' data:; style-src 'self'; script-src 'self'; connect-src 'self'; frame-ancestors 'none'; base-uri 'self'; form-action 'self'";
            ctx.Response.Headers.CacheControl = "no-store";
            try
            {
                await next();
            }
            catch (UnauthorizedAccessException e)
            {
                ctx.Response.StatusCode = 403;
                await ctx.Response.WriteAsJsonAsync(new { error = e.Message });
            }
            catch (AntiforgeryValidationException)
            {
                ctx.Response.StatusCode = 400;
                await ctx.Response.WriteAsJsonAsync(new { error = "Güvenlik doğrulaması yenilenmeli. Sayfayı yenileyin." });
            }
            catch (Exception e)when (e is InvalidOperationException or InvalidDataException or ArgumentException or JsonException or CryptographicException)
            {
                ctx.Response.StatusCode = 409;
                await ctx.Response.WriteAsJsonAsync(new { error = e is CryptographicException ? "Sayım taslağı doğrulanamadı. Yeni sayım başlatın." : e.Message });
            }
            catch (DbUpdateException)
            {
                ctx.Response.StatusCode = 409;
                await ctx.Response.WriteAsJsonAsync(new { error = "İşlem mevcut kayıtlarla çakıştı. Sayfayı yenileyip tekrar deneyin." });
            }
            catch (Exception e)
            {
                app.Logger.LogError(e, "Mobil işlem tamamlanamadı");
                ctx.Response.StatusCode = 503;
                await ctx.Response.WriteAsJsonAsync(new { error = "Veritabanına ulaşılamadı. Masaüstü SQL bağlantısını ve sunucu hizmetini kontrol edin. Taslağınız korunuyor." });
            }
        });
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseRateLimiter();
        app.UseAuthentication();
        app.UseAuthorization();
        app.Use(async (ctx, next) =>
        {
            if (ctx.Request.Path.StartsWithSegments("/api") && HttpMethods.IsPost(ctx.Request.Method))
                await ctx.RequestServices.GetRequiredService<IAntiforgery>().ValidateRequestAsync(ctx);
            if (ctx.User.Identity?.IsAuthenticated == true && ctx.Request.Path.StartsWithSegments("/api"))
            {
                var data = ctx.RequestServices.GetRequiredService<IInventoryPersistence>().Export();
                var id = ctx.User.EmployeeId();
                var account = data.Accounts.SingleOrDefault(a => a.EmployeeId == id && a.Enabled);
                if (account is null || AccountCredentials.Stamp(account) != ctx.User.FindFirst("credential")?.Value)
                {
                    await ctx.SignOutAsync();
                    ctx.Response.StatusCode = 401;
                    return;
                }

                MobileWorkspace.Person(data, id);
            }

            await next();
        });
    }
}
