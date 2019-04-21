using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public interface ITransactionData
    {
        IDictionary<DateTime, int> IncomePerDay { get; set; }
        IDictionary<DateTime, IPurchase> PurchasePerDay { get; set; }
        int TotalIncome { get; set; }
        int TotalPurchase { get; set; }
        IDictionary<DateTime, IProductTransfer> TransferPerDay { get; set; }
    }
}
