namespace CafeStockAI.Desktop;

internal static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => ReportError(e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) => ReportError(e.ExceptionObject as Exception ?? new Exception(e.ExceptionObject.ToString()));
        try
        {
            ApplicationConfiguration.Initialize();
            if (args.Length == 2 && args[0] == "--check-system")
            {
                try { SystemCheck.Run(args[1]); }
                catch (Exception ex) { Directory.CreateDirectory(args[1]); File.WriteAllText(Path.Combine(args[1], "result.txt"), ex.ToString()); Environment.ExitCode = 1; }
                return;
            }
            if (args.Length == 2 && args[0] == "--check-features") { FeatureCheck.Run(args[1]); return; }
            if (args.Length == 2 && args[0] == "--check-startup")
            {
                try
                {
                    using var login = new FrmLogin();
                    using var dashboard = new FrmDashboard();
                    using var products = new FrmProducts();
                    login.CreateControl(); dashboard.CreateControl(); products.CreateControl();
                    dashboard.CheckAllPages(); dashboard.ShowProducts(); dashboard.ShowOverview();
                    File.WriteAllText(args[1], "OK: Giriş, ana panel ve ürün ekranları oluşturuldu.");
                }
                catch (Exception ex) { File.WriteAllText(args[1], ex.ToString()); Environment.ExitCode = 1; }
                return;
            }
            using var applicationMutex=new Mutex(true,"Local\\CafeStockAI.Application",out bool firstInstance);
            if(!firstInstance){MessageBox.Show("CafeStock AI zaten açık. Açık uygulamayı kullanın veya önce kapatın.");return;}
            DemoData.Initialize();
            Accounts.Sync();
            string dailyBackup=Path.Combine(InventoryStore.DirectoryPath,"Yedekler");
            if(!Directory.Exists(dailyBackup)||!Directory.EnumerateFiles(dailyBackup,"CafeStock-"+DateTime.Now.ToString("yyyyMMdd")+"-*.zip").Any())Operations.CreateBackup();
            Application.Run(new FrmLogin());
        }
        catch (Exception ex) { ReportError(ex); }
    }

    private static void ReportError(Exception exception)
    {
        string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CafeStockAI", "error.log");
        string details;
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(logPath)!);
            File.AppendAllText(logPath, $"{DateTimeOffset.Now:O}\n{exception}\n\n");
            details = "Hata ayrıntıları: " + logPath;
        }
        catch { details = exception.ToString(); }
        MessageBox.Show("Uygulamada bir hata oluştu:\n" + exception.Message + "\n\n" + details,
            "CafeStock AI - Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
