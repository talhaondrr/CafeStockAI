using CafeStockAI.Application;
using CafeStockAI.Application.Mobile;
using CafeStockAI.Application.Analysis;

namespace CafeStockAI.Mobile;
internal static class WorkspaceEndpoints
{
    public static void MapWorkspaceEndpoints(this WebApplication app)
    {
        app.MapGet("/images/{file}", (string file) =>
        {
            var assembly = typeof(DemoPersistence).Assembly;
            var resource = "Mobile.Images." + file;
            if (!assembly.GetManifestResourceNames().Contains(resource))
                return Results.NotFound();
            return Results.File(assembly.GetManifestResourceStream(resource)!, "image/png");
        });
        app.MapGet("/api/workspace", (HttpContext c, MobileWorkspace service) =>
        {
            var a = service.Snapshot(c.User.EmployeeId());
            var person = MobileWorkspace.Person(a, c.User.EmployeeId());
            var stocks = a.Products.Select(p => new { p.Id, p.Name, p.Category, p.Unit, p.MinimumStock, image = "/images/" + ProductImageKey.For(p) + ".png", bar = MobileWorkspace.Balance(a.Inventory, p.Id, "Bar"), depot = MobileWorkspace.Balance(a.Inventory, p.Id, "Depo") }).ToList();
            var counts = a.Inventory.Counts.Where(v => v.BranchName == "Elazığ" && (MobileWorkspace.Approver(person) || v.Operator == person.Name)).OrderByDescending(v => v.Date).Take(100).Select(v => new { v.Id, v.Date, v.Operator, v.Status, v.Reviewer, v.ReviewNote, v.ReviewedAt, area = CountApproval.Area(v), lines = v.Lines.Select(l => new { l.ProductId, name = a.Products.FirstOrDefault(p => p.Id == l.ProductId)?.Name ?? "Kaldırılmış ürün", unit = a.Products.FirstOrDefault(p => p.Id == l.ProductId)?.Unit ?? "", l.BarExpected, l.DepotExpected, l.BarCounted, l.DepotCounted }) });
            var areas = a.Inventory.Warehouses.ToDictionary(w => w.Id, w => w.StockArea);
            var movements = MobileWorkspace.Movement(person) ? a.Inventory.Movements.OrderByDescending(m => m.Date).Take(100).Select(m => new { m.Id, m.Date, m.Type, m.Quantity, m.Note, area = areas.GetValueOrDefault(m.WarehouseId, ""), name = a.Products.FirstOrDefault(p => p.Id == m.ProductId)?.Name ?? "Kaldırılmış ürün" }).ToArray() : [];
            var analysis = MobileWorkspace.Approver(person)
                ? stocks.Select(p =>
                {
                    decimal stock = p.bar + p.depot;
                    var forecast = DemandModel.Train(a.Inventory, p.Id, "Elazığ", sampleData: c.RequestServices.GetRequiredService<IInventoryPersistence>() is DemoPersistence);
                    var plan = PurchasePlan.Create(p.Unit, stock, p.MinimumStock, forecast);
                    return new
                    {
                        p.Id, p.Name, p.Unit, stock, p.MinimumStock, plan.Priority, plan.Reason, plan.Rank,
                        sevenDays = plan.Demand, plan.Reorder, plan.Remaining,
                        mae = forecast?.Error, days = forecast?.Samples, series = forecast?.Series,
                        explanation = plan.Explain(p.Unit, stock, p.MinimumStock, forecast)
                    };
                }).OrderBy(p => p.Rank).ThenByDescending(p => p.Reorder).ThenBy(p => p.Name).ToArray()
                : [];
            var staff = MobileWorkspace.Global(person) ? a.Inventory.Employees.Where(e => e.IsActive).Select(e => new { e.Id, e.Name, e.Role, e.Phone, e.HireDate, enabled = a.Accounts.Any(u => u.EmployeeId == e.Id && u.Enabled), username = a.Accounts.FirstOrDefault(u => u.EmployeeId == e.Id)?.Username }).ToArray() : [];
            return Results.Ok(new { stocks, counts, movements, analysis, leadDays = PurchasePlan.LeadDays, staff, roles = MobileWorkspace.Roles, business = a.Inventory.Settings.BusinessName });
        }).RequireAuthorization();
    }
}
