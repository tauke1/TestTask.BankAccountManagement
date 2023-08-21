﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.DAL.Contexts;
using TestTask.BankAccountManagement.DAL.Models;
using TestTask.BankAccountManagement.DAL.Repositories.Interfaces;

namespace TestTask.BankAccountManagement.DAL.Repositories
{
    public class AccountOperationRepository : BaseRepository<AccountOperation>, IAccountOperationRepository
    {
        public AccountOperationRepository(AppDbContext context) : base(context)
        {
        }
    }
}
