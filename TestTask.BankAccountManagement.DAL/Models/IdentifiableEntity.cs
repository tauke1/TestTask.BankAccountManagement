using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.DAL.Models.Interfaces;

namespace TestTask.BankAccountManagement.DAL.Models
{
    public abstract class IdentifiableEntity : IIdentifiable
    {
        public long Id { get; set; }
    }
}
