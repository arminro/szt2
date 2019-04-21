using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public interface IEmployee
    {
        int EmpID { get; set; }
        string  Email { get; set; }
        string Password { get; set; }
        EmployeeRole Role { get; set; }
        IEnumerable<ITransaction> Transactions { get; set; }
        string Username { get; set; } 
    }
}
