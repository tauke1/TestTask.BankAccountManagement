using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.DAL.Models.Enums;
using TestTask.BankAccountManagement.DAL.Models.Interfaces;

namespace TestTask.BankAccountManagement.DAL.Repositories.Interfaces
{
    public interface IRepository<T> where T : class, IIdentifiable
    {
        Task<T> SingleOrDefaultByIdAsync(long id);
        Task<T> SingleOrDefaultByFilterAsync(
            Expression<Func<T, bool>> expression,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include = null);
        Task<List<T>> SelectByFilterAsync(
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include = null);
        Task<List<T>> SelectByFilterAndPaginationAsync(
            int pageNumber = 1,
            int pageSize = 1,
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include = null);
        Task<List<T>> SelectByFilterAsync(
            Expression<Func<T, bool>> predicate = null,
            string orderBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include = null);
        Task<List<T>> SelectByFilterAndPaginationAsync(
            int pageNumber = 1,
            int pageSize = 1,
            Expression<Func<T, bool>> predicate = null,
            string orderBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include = null);
        Task<int> CountByFilterAsync(
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include = null);

        Task AddAsync(T @object);
        Task AddRangeAsync(params T[] @object);
        void Update(T @object);
        void UpdateRange(params T[] objects);
        void Remove(T @object);
        void RemoveRange(IEnumerable<T> objects);
    }
}
