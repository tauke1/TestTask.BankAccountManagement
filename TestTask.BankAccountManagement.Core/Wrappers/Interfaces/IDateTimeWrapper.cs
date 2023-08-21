using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.BankAccountManagement.Core.Wrappers.Interfaces
{
    public interface IDateTimeWrapper
    {
        DateTime UtcNow { get; }
        DateTime Now { get; }
    }
}
