using System.Text.RegularExpressions;
namespace CafeStockAI.Desktop;

internal static class PhoneNumber
{
    public static string Format(string? value)
    {
        string input = (value ?? "").Trim();
        if (input.Length == 0) return "";
        if (!Regex.IsMatch(input, @"^\+?[0-9\s()\-]+$"))
            throw new InvalidOperationException("Telefon numarası yalnızca rakam, boşluk, parantez ve başta + işareti içerebilir.");
        string digits = new string(input.Where(c => c >= '0' && c <= '9').ToArray());
        if (digits.Length == 14 && digits.StartsWith("0090")) digits = digits[4..];
        else if (digits.Length == 12 && digits.StartsWith("90")) digits = digits[2..];
        else if (digits.Length == 11 && digits.StartsWith("0")) digits = digits[1..];
        if (digits.Length != 10 || digits[0] == '0' || digits[0] == '1')
            throw new InvalidOperationException("Geçerli bir Türkiye telefon numarası giriniz. Örnek biçim: 0 (5xx) xxx xx xx.");
        return $"0 ({digits[..3]}) {digits.Substring(3,3)} {digits.Substring(6,2)} {digits.Substring(8,2)}";
    }
}
