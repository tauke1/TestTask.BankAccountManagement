using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.BankAccountManagement.DAL.Models.Interfaces
{
    public interface ISoftDeletable
    {
        DateTime? DeletedAt { get; set; }
    }
}
