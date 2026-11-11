using CafeStockAI.Application;
using CafeStockAI.Application.Models;

namespace CafeStockAI.Mobile;
sealed class UnavailablePersistence : IInventoryPersistence
{
    private static Exception Missing() => new InvalidOperationException("Önce masaüstünde SQL bağlantısını kurun veya Visual Studio'da Mobil Demo profilini seçin.");
    public InventoryData LoadInventory() => throw Missing();
    public List<Product> LoadProducts() => throw Missing();
    public List<UserAccount> LoadAccounts() => throw Missing();
    public InventoryArchive Export() => throw Missing();
    public void SaveInventory(InventoryData d) => throw Missing();
    public void SaveProducts(List<Product> d) => throw Missing();
    public void SaveAccounts(List<UserAccount> d) => throw Missing();
    public void Restore(InventoryArchive d) => throw Missing();
}
