using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.DAL.Models.Interfaces;

namespace TestTask.BankAccountManagement.DAL.Models
{
    public abstract class SoftDeletableEntity : IdentifiableEntity, ISoftDeletable
    {
        public DateTime? DeletedAt { get; set; }
    }
}
