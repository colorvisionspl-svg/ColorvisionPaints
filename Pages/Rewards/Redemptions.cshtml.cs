using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ColorvisionPaintsERP.Models;
using ColorvisionPaintsERP.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System;

namespace ColorvisionPaintsERP.Pages.Rewards
{
    [Authorize]
    public class RedemptionsModel : PageModel
    {
        private readonly IRewardsService _svc;
        public RedemptionsModel(IRewardsService svc) => _svc = svc;

        public List<PainterTransaction> Txns { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Txns = await _svc.GetTransactionsAsync("");
            Txns = Txns.Where(t => t.Type == TxnType.Redeem).ToList();

            if (Txns.Count == 0)
                Txns = new List<PainterTransaction>
                {
                    new PainterTransaction { Id="T1", PainterName="Mohan Das",  Points=5000, Amount=5000, RedemptionType="UPI", UpiId="mohan@paytm",  Status=TxnStatus.Pending,    CreatedAt=DateTime.Now.AddHours(-2) },
                    new PainterTransaction { Id="T2", PainterName="Ravi Shankar",Points=2500, Amount=2250, RedemptionType="UPI", UpiId="ravi@upi",    Status=TxnStatus.Processing, CreatedAt=DateTime.Now.AddDays(-1),  TdsDeducted=250 },
                    new PainterTransaction { Id="T3", PainterName="Suresh Patil", Points=1000, Amount=1000, RedemptionType="UPI", UpiId="suresh@gpay", Status=TxnStatus.Completed,  CreatedAt=DateTime.Now.AddDays(-3),  UpiTransactionId="UPI-XXXX-7890" },
                    new PainterTransaction { Id="T4", PainterName="Anil Kumar",  Points=500,  Amount=500,  RedemptionType="UPI", UpiId="anil@ybl",    Status=TxnStatus.Failed,     CreatedAt=DateTime.Now.AddDays(-2) },
                };
            return Page();
        }
    }
}
