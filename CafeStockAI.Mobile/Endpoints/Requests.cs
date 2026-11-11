using CafeStockAI.Application.Mobile;

namespace CafeStockAI.Mobile;
record LoginInput(string Username, string Password);
record BeginCountInput(string Area);
record SubmitInput(string Ticket, List<CountEntry> Lines);
record ReviewInput(Guid Id, bool Approved, string? Note);
record IdInput(Guid Id);
record SettingsInput(string Business);
