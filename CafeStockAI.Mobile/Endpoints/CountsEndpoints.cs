using System.Text.Json;
using CafeStockAI.Application.Mobile;
using Microsoft.AspNetCore.DataProtection;

namespace CafeStockAI.Mobile;
internal static class CountsEndpoints
{
    public static void MapCountsEndpoints(this WebApplication app, RouteGroupBuilder commands)
    {
        commands.MapPost("/api/counts/begin", (BeginCountInput input, HttpContext c, MobileWorkspace service, IDataProtectionProvider dp) =>
        {
            if (input.Area is not ("Bar" or "Depo"))
                throw new InvalidOperationException("Bar veya Depo seçin.");
            var ticket = service.Begin(c.User.EmployeeId(), input.Area);
            return Results.Ok(new { ticket = dp.CreateProtector("count-ticket-v2").Protect(JsonSerializer.Serialize(ticket)), ticket.Id, ticket.Expires, area = ticket.Area });
        }).RequireAuthorization();
        commands.MapPost("/api/counts/submit", (SubmitInput input, HttpContext c, MobileWorkspace service, IDataProtectionProvider dp) =>
        {
            var ticket = JsonSerializer.Deserialize<CountTicket>(dp.CreateProtector("count-ticket-v2").Unprotect(input.Ticket)) ?? throw new InvalidDataException("Geçersiz taslak.");
            var id = service.Submit(c.User.EmployeeId(), ticket, input.Lines);
            return Results.Ok(new { id, status = "Onay bekliyor" });
        }).RequireAuthorization();
        commands.MapPost("/api/counts/review", (ReviewInput input, HttpContext c, MobileWorkspace service) =>
        {
            service.Review(c.User.EmployeeId(), input.Id, input.Approved, input.Note);
            return Results.Ok(new { ok = true });
        }).RequireAuthorization();
    }
}
