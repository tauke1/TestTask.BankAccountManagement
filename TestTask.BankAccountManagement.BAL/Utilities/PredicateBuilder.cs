using System;
using System.Linq.Expressions;
using TestTask.BankAccountManagement.BAL.Extensions;
using TestTask.BankAccountManagement.BAL.Utilities.Interfaces;

namespace TestTask.BankAccountManagement.BAL.Utilities
{
    public class PredicateBuilder : IPredicateBuilder
    {
        public Expression<Func<T, bool>> And<T>(params Expression<Func<T, bool>>[] predicates)
        {
            Expression<Func<T, bool>> finalPredicate = null;
            foreach (var predicate in predicates)
            {
                if (predicate == null)
                {
                    continue;
                }

                if (finalPredicate == null)
                {
                    finalPredicate = predicate;
                    continue;
                }

                finalPredicate = finalPredicate.And(predicate);
            }

            return finalPredicate;
        }

        public Expression<Func<T, bool>> Or<T>(params Expression<Func<T, bool>>[] predicates)
        {
            Expression<Func<T, bool>> finalPredicate = null;
            foreach (var predicate in predicates)
            {
                if (predicate == null)
                {
                    continue;
                }

                if (finalPredicate == null)
                {
                    finalPredicate = predicate;
                    continue;
                }

                finalPredicate = finalPredicate.Or(predicate);
            }

            return finalPredicate;
        }
    }
}
