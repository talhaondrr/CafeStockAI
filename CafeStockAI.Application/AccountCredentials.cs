using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using CafeStockAI.Application.Models;

namespace CafeStockAI.Application;
public static class AccountCredentials
{
    public static string Key(string name) => name.Trim().Normalize(NormalizationForm.FormKC).ToUpper(CultureInfo.GetCultureInfo("tr-TR"));
    public static bool Verify(UserAccount account, string password)
    {
        try
        {
            return CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(account.Hash), Rfc2898DeriveBytes.Pbkdf2(password, Convert.FromBase64String(account.Salt), 210000, HashAlgorithmName.SHA256, 32));
        }
        catch (FormatException)
        {
            return false;
        }
    }

    public static void Set(UserAccount account, string password)
    {
        if (password.Length < 4 || password.Length > 128)
            throw new InvalidOperationException("Şifre 4–128 karakter olmalı.");
        account.Salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        account.Hash = Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(password, Convert.FromBase64String(account.Salt), 210000, HashAlgorithmName.SHA256, 32));
    }

    public static string Stamp(UserAccount account) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(account.Hash)));
}
