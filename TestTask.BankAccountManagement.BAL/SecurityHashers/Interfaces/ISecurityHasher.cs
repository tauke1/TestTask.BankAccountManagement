﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.BankAccountManagement.BAL.SecurityHashers.Interfaces
{
    public interface ISecurityHasher
    {
         string Hash(string key);

         bool Verify(string key, string hashed);
    }
}
