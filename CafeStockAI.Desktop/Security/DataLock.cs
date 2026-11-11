using CafeStockAI.Application;
using System.Globalization;
using System.Text.Json;

namespace CafeStockAI.Desktop;
internal sealed class DataLock : IDisposable
{
    private readonly Mutex mutex;
    private DataLock()
    {
        mutex = new Mutex(false, "Local\\CafeStockAI.Data.v3");
        try
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(15)))
                throw new IOException("Başka bir işlem kayıt yapıyor. Yeniden deneyin.");
        }
        catch (AbandonedMutexException)
        {
        }
    }

    public static DataLock Enter() => new();
    public void Dispose()
    {
        mutex.ReleaseMutex();
        mutex.Dispose();
    }
}
