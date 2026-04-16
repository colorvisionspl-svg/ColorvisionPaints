using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ColorvisionPaintsERP.Pages.Procurement
{
    [Authorize]
    public class PurchaseBillsModel : PageModel
    {
        private readonly IProcurementService _procurementService;

        public PurchaseBillsModel(IProcurementService procurementService)
        {
            _procurementService = procurementService;
        }

        public List<PurchaseBill> Bills { get; set; } = new();
        public List<GRN> ApprovedGRNs { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            string idToken = ""; 

            Bills = await _procurementService.GetBillsAsync(idToken);
            var allGRNs = await _procurementService.GetGRNsAsync(idToken);
            ApprovedGRNs = allGRNs.Where(g => g.Status == GRNStatus.QCApproved).ToList();
            
            if (Bills.Count == 0)
            {
                Bills = new List<PurchaseBill>
                {
                    new PurchaseBill { 
                        BillNumber = "BILL-2025-001", 
                        GrnId = "CV/GRN/2025-26/0001", 
                        VendorId = "VND-0001", 
                        VendorInvoiceNumber = "INV/CHEM/982",
                        TotalAmount = 45000, 
                        MatchStatus = "Matched",
                        PaymentStatus = "Paid"
                    },
                    new PurchaseBill { 
                        BillNumber = "BILL-2025-002", 
                        GrnId = "CV/GRN/2025-26/0002", 
                        VendorId = "VND-0002", 
                        VendorInvoiceNumber = "PIG/HUB/441",
                        TotalAmount = 125000, 
                        MatchStatus = "Mismatch",
                        PaymentStatus = "Unpaid"
                    }
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSaveBillAsync([FromBody] PurchaseBill bill)
        {
            string idToken = "";
            await _procurementService.CreateBillAsync(bill, idToken);
            return new JsonResult(new { success = true });
        }
    }
}
