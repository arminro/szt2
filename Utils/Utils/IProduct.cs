using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public interface IProduct
    {
        string Name { get; set; }
        int Price { get; set; }
        int ProductID { get; set; }
        IShop Shop { get; set; }
        IEnumerable<ITransaction> Transactions { get; set; }
        Type Type { get; set; }

    }
}
