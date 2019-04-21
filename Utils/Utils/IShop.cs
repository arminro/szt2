using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public interface IShop
    {
        int ShopID { get; set; }
        string Location { get; set; }
        IEnumerable<IProduct> Products { get; set; }
        IEnumerable<ITransaction> TransactionsFromHere { get; set; }
        IEnumerable<IProduct> TransactionsArrivingHere { get; set; }
      
       
    }
}
