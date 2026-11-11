using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace CafeStockAI.Infrastructure;
public sealed record SqlConnectionSettings(string Server, string Database)
{
    public static SqlConnectionSettings Load(string path) => JsonSerializer.Deserialize<SqlConnectionSettings>(File.ReadAllText(path)) ?? throw new InvalidDataException("SQL bağlantı ayarı geçersiz.");
    public string ConnectionString(string applicationName)
    {
        if (string.IsNullOrWhiteSpace(Server) || string.IsNullOrWhiteSpace(Database))
            throw new InvalidOperationException("Sunucu ve veritabanı adı giriniz.");
        string host = Server.Split('\\', ',')[0];
        bool local = new[]
        {
            ".",
            "localhost",
            "(local)",
            Environment.MachineName
        }.Contains(host, StringComparer.OrdinalIgnoreCase);
        return new SqlConnectionStringBuilder
        {
            DataSource = Server.Trim(),
            InitialCatalog = Database.Trim(),
            IntegratedSecurity = true,
            Encrypt = true,
            TrustServerCertificate = local,
            ConnectTimeout = 5,
            ApplicationName = applicationName
        }.ConnectionString;
    }
}
