using IbanNet.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.BankAccountManagement.BAL.Factories.Interfaces
{
    public interface IIbanGeneratorFactory
    {
        IIbanGenerator Create();
    }
}
