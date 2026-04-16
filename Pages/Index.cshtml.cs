using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;

namespace ColorvisionPaintsERP.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToAction("Login", "Auth");
            }

            var role = User.FindFirst("Role")?.Value ?? string.Empty;
            return role switch
            {
                UserRoles.SuperAdmin or UserRoles.FinanceManager => Redirect("/Dashboard/Admin"),
                UserRoles.SalesHead or UserRoles.AreaManager => Redirect("/Dashboard/Sales"),
                UserRoles.SalesRep => Redirect("/Dashboard/Salesman"),
                UserRoles.WarehouseManager => Redirect("/Dashboard/Warehouse"),
                _ => Redirect("/Dashboard/Dealer")
            };
        }
    }
}
