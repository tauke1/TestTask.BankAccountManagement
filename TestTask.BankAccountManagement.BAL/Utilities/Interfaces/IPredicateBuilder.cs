using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestTask.BankAccountManagement.BAL.Utilities.Interfaces
{
    public interface IPredicateBuilder
    {
        Expression<Func<T, bool>> And<T>(params Expression<Func<T, bool>>[] predicates);
        Expression<Func<T, bool>> Or<T>(params Expression<Func<T, bool>>[] predicates);
    }
}
