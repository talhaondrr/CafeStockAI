using CafeStockAI.Application.Models;

namespace CafeStockAI.Application.Mobile;
public sealed record CountExpected(Guid ProductId, string Unit, decimal Bar, decimal Depot);
public sealed record CountTicket(Guid Id, Guid EmployeeId, DateTimeOffset Expires, List<CountExpected> Products, string Area = "Both");
public sealed record CountEntry(Guid ProductId, decimal? Bar, decimal? Depot);
public sealed record ProductEdit(Guid? Id, string Name, string Category, string Unit, decimal MinimumStock);
public sealed record StaffEdit(Guid? Id, string Name, string Role, string Phone, DateTime? HireDate);
public sealed record AccountEdit(Guid EmployeeId, bool Enabled, string? Password);
public sealed record MovementEdit(Guid Id, Guid ProductId, string Area, string Type, decimal Quantity, string Note);
