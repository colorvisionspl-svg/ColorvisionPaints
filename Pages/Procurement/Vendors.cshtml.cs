using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ColorvisionPaintsERP.Pages.Procurement
{
    [Authorize]
    public class VendorsModel : PageModel
    {
        private readonly IProcurementService _procurementService;

        public VendorsModel(IProcurementService procurementService)
        {
            _procurementService = procurementService;
        }

        public List<Vendor> Vendors { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Note: In a real app, we'd get the idToken from a cookie or session.
            // For this UI-focused build, we'll assume the service handles empty tokens gracefully 
            // or we'll mock the data if Firestore is not reachable.
            string idToken = ""; 

            Vendors = await _procurementService.GetVendorsAsync(idToken);
            
            // Mock data for UI demonstration if list is empty
            if (Vendors.Count == 0)
            {
                Vendors = new List<Vendor>
                {
                    new Vendor { VendorCode = "VND-0001", CompanyName = "ChemPrime Solutions", ContactPerson = "Rahul Mehta", City = "Mumbai", Mobile = "9820012345", QualityRating = 5, IsActive = true },
                    new Vendor { VendorCode = "VND-0002", CompanyName = "Pigment Hub Ltd", ContactPerson = "Anita Desai", City = "Ahmedabad", Mobile = "9930054321", QualityRating = 4, IsActive = true },
                    new Vendor { VendorCode = "VND-0003", CompanyName = "Resin Tech India", ContactPerson = "Suresh Pal", City = "Chennai", Mobile = "9123456789", QualityRating = 3, IsActive = false }
                };
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddVendorAsync([FromBody] Vendor vendor)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            string idToken = "";
            vendor.VendorCode = await _procurementService.GetNextSequenceIdAsync("vendors", "VND", idToken);
            await _procurementService.CreateVendorAsync(vendor, idToken);
            
            return new JsonResult(new { success = true });
        }
    }
}
