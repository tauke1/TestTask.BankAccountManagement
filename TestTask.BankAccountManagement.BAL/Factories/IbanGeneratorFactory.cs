using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IbanNet.Registry;
using TestTask.BankAccountManagement.BAL.Factories.Interfaces;

namespace TestTask.BankAccountManagement.BAL.Factories
{
    public class IbanGeneratorFactory : IIbanGeneratorFactory
    {
        public IIbanGenerator Create()
        {
            return new IbanGenerator();
        }
    }
}
