using System.Security.Claims;

namespace CafeStockAI.Mobile;
internal static class CurrentUser
{
    public static Guid EmployeeId(this ClaimsPrincipal user) => Guid.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : throw new UnauthorizedAccessException("Önce sisteme giriş yapın.");
}
