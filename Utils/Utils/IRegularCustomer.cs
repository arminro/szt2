using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public interface IRegularCustomer
    {
        string Email { get; set; }
        string Name { get; set; }
        int RegularCustomerID { get; set; }
        IEnumerable<IPurchase> Transactions { get; set; }
    }
}
