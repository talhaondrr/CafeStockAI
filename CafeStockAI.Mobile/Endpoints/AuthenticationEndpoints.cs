using System.Security.Claims;
using CafeStockAI.Application;
using CafeStockAI.Application.Mobile;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CafeStockAI.Mobile;
internal static class AuthenticationEndpoints
{
    public static void MapAuthenticationEndpoints(this WebApplication app, RouteGroupBuilder commands, bool demo)
    {
        app.MapGet("/api/session", (HttpContext c, IAntiforgery af, IInventoryPersistence store) =>
        {
            var csrf = af.GetAndStoreTokens(c).RequestToken;
            if (c.User.Identity?.IsAuthenticated != true)
                return Results.Ok(new { authenticated = false, csrf, demo });
            var a = store.Export();
            var p = MobileWorkspace.Person(a, c.User.EmployeeId());
            return Results.Ok(new { authenticated = true, csrf, demo, person = new { id = p.Id, p.Name, p.Role, branch = "Elazığ", manage = MobileWorkspace.Global(p), approve = MobileWorkspace.Approver(p), movement = MobileWorkspace.Movement(p) }, business = a.Inventory.Settings.BusinessName });
        });
        commands.MapPost("/api/login", async (LoginInput input, HttpContext c, IInventoryPersistence store) =>
        {
            if (string.IsNullOrWhiteSpace(input.Username) || input.Username.Length > 160 || string.IsNullOrEmpty(input.Password) || input.Password.Length > 128)
                return Results.BadRequest(new { error = "Kullanıcı adı ve şifre girin." });
            var a = store.Export();
            var user = a.Accounts.FirstOrDefault(u => u.Enabled && AccountCredentials.Key(u.Username) == AccountCredentials.Key(input.Username));
            if (user is null || !AccountCredentials.Verify(user, input.Password))
                return Results.Json(new { error = "Kullanıcı adı veya şifre hatalı." }, statusCode: 401);
            var person = MobileWorkspace.Person(a, user.EmployeeId);
            await c.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, person.Id.ToString()), new Claim("credential", AccountCredentials.Stamp(user)) }, CookieAuthenticationDefaults.AuthenticationScheme)));
            return Results.Ok(new { ok = true });
        }).RequireRateLimiting("login");
        commands.MapPost("/api/logout", async (HttpContext c) =>
        {
            await c.SignOutAsync();
            return Results.Ok(new { ok = true });
        }).RequireAuthorization();
    }
}
