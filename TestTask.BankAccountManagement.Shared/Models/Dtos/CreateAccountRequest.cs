using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.Shared.Models.Dtos.Enums;

namespace TestTask.BankAccountManagement.Shared.Models.Dtos
{
    public class CreateAccountRequest
    {
        public AccountTypeDto Type { get; set; }

        public long CountryId { get; set; }

        public decimal StartingBalance { get; set; }
    }
}
