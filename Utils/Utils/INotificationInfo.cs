using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public interface INotificationInfo
    {
        string Message { get; set; }
        string NotificationSubject { get; set; }
        IEnumerable<string> Recipients { get; set; }
        string SenderEmail { get; set; }
        string SenderName { get; set; }
    }
}
