using System.Collections.Generic;
using System.Threading.Tasks;
using ColorvisionPaintsERP.Models;

namespace ColorvisionPaintsERP.Services
{
    public interface IProcurementService
    {
        // Vendors
        Task<List<Vendor>> GetVendorsAsync(string idToken);
        Task<Vendor?> GetVendorAsync(string id, string idToken);
        Task CreateVendorAsync(Vendor vendor, string idToken);
        Task UpdateVendorAsync(string id, Vendor vendor, string idToken);

        // Raw Materials
        Task<List<RawMaterial>> GetRawMaterialsAsync(string idToken);
        Task CreateRawMaterialAsync(RawMaterial material, string idToken);

        // Purchase Orders
        Task<List<PurchaseOrder>> GetPurchaseOrdersAsync(string idToken);
        Task<PurchaseOrder?> GetPurchaseOrderAsync(string id, string idToken);
        Task CreatePurchaseOrderAsync(PurchaseOrder po, string idToken);
        Task UpdatePurchaseOrderStatusAsync(string id, string status, string idToken);

        // GRN
        Task<List<GRN>> GetGRNsAsync(string idToken);
        Task CreateGRNAsync(GRN grn, string idToken);
        Task UpdateGRNStatusAsync(string id, string status, string idToken);

        // Bills
        Task<List<PurchaseBill>> GetBillsAsync(string idToken);
        Task CreateBillAsync(PurchaseBill bill, string idToken);

        // Stock
        Task<List<RawMaterialStock>> GetStockAsync(string idToken);
        Task UpdateStockStatusAsync(string id, string status, string idToken);
        
        // Sequential ID Helpers
        Task<string> GetNextSequenceIdAsync(string collection, string prefix, string idToken);
    }
}
