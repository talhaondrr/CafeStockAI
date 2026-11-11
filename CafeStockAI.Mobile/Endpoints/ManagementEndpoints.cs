using CafeStockAI.Application.Mobile;

namespace CafeStockAI.Mobile;
internal static class ManagementEndpoints
{
    public static void MapManagementEndpoints(this WebApplication app, RouteGroupBuilder commands)
    {
        commands.MapPost("/api/movements", (MovementEdit input, HttpContext c, MobileWorkspace service) =>
        {
            service.AddMovement(c.User.EmployeeId(), input);
            return Results.Ok(new { ok = true });
        }).RequireAuthorization();
        commands.MapPost("/api/products", (ProductEdit input, HttpContext c, MobileWorkspace service) =>
        {
            service.SaveProduct(c.User.EmployeeId(), input);
            return Results.Ok(new { ok = true });
        }).RequireAuthorization();
        commands.MapPost("/api/products/remove", (IdInput input, HttpContext c, MobileWorkspace service) =>
        {
            service.RemoveProduct(c.User.EmployeeId(), input.Id);
            return Results.Ok(new { ok = true });
        }).RequireAuthorization();
        commands.MapPost("/api/staff", (StaffEdit input, HttpContext c, MobileWorkspace service) =>
        {
            service.SaveStaff(c.User.EmployeeId(), input);
            return Results.Ok(new { ok = true });
        }).RequireAuthorization();
        commands.MapPost("/api/staff/remove", (IdInput input, HttpContext c, MobileWorkspace service) =>
        {
            service.RemoveStaff(c.User.EmployeeId(), input.Id);
            return Results.Ok(new { ok = true });
        }).RequireAuthorization();
        commands.MapPost("/api/accounts", (AccountEdit input, HttpContext c, MobileWorkspace service) =>
        {
            service.ChangeAccount(c.User.EmployeeId(), input);
            return Results.Ok(new { ok = true });
        }).RequireAuthorization();
        commands.MapPost("/api/settings", (SettingsInput input, HttpContext c, MobileWorkspace service) =>
        {
            service.SaveSettings(c.User.EmployeeId(), input.Business);
            return Results.Ok(new { ok = true });
        }).RequireAuthorization();
    }
}
