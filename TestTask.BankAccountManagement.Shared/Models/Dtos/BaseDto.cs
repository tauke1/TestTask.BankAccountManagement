using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.Shared.Models.Dtos.Interfaces;

namespace TestTask.BankAccountManagement.Shared.Models.Dtos
{
    public class BaseDto : IIdentifiableDto
    { 
        public long Id { get; set; }
    }
}
