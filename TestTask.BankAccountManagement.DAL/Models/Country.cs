﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.BankAccountManagement.DAL.Models
{
    public class Country : IdentifiableEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
