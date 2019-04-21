using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public interface ITransaction
    {
        DateTime DateOfTransaction { get; set; }
        IEmployee Emp { get; set; }
        IEnumerable<IProduct> Products { get; set; }
        int TransactionID { get; set; }
        IShop Shop { get; set; }
    }
}
