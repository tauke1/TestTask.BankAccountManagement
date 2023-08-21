using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IbanNet.Builders;
using IbanNet.Registry;
using TestTask.BankAccountManagement.BAL.Wrappers.Interfaces;

namespace TestTask.BankAccountManagement.BAL.Wrappers
{
    public class RandomWrapper : IRandomWrapper
    {
        public Guid GenerateGuid() => Guid.NewGuid();
    }
}
