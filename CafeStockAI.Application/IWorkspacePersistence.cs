using CafeStockAI.Application.Models;
namespace CafeStockAI.Application;
public interface IWorkspacePersistence
{
 void SaveStaffAndAccounts(InventoryData data,List<UserAccount> accounts);
 void SaveProductsChecked(List<Product> products,byte[] version);
 void SaveAccountsChecked(List<UserAccount> accounts,byte[] version);
}
