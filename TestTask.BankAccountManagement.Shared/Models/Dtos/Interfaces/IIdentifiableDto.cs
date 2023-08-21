using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.BankAccountManagement.Shared.Models.Dtos.Interfaces
{
    public interface IIdentifiableDto
    {
        public long Id { get; set; }
    }
}
