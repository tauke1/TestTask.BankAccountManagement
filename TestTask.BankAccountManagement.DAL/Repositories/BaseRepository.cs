using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestTask.BankAccountManagement.DAL.Contexts;
using TestTask.BankAccountManagement.DAL.Models.Enums;
using TestTask.BankAccountManagement.DAL.Models.Interfaces;
using TestTask.BankAccountManagement.DAL.Repositories.Interfaces;

namespace TestTask.BankAccountManagement.DAL.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IIdentifiable
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> SingleOrDefaultByIdAsync(
            long id)
        {
            var @object = await _dbSet.FindAsync(id);
            if (@object != null)
            {
                // detach because general approach is turn off tracking
                _context.Entry(@object).State = EntityState.Detached;
            }

            return @object;
        }

        public Task<T> SingleOrDefaultByFilterAsync(
            Expression<Func<T, bool>> predicate,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include = null)
        {
            var query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include.Compile()(query);
            }

            return query.SingleOrDefaultAsync();
        }

        public Task<List<T>> SelectByFilterAsync(
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include = null)
        {
            return GetSelectByFilterQuery(predicate, orderBy, include).ToListAsync();
        }

        public Task<List<T>> SelectByFilterAndPaginationAsync(
            int pageNumber = 1,
            int pageSize = 1,
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy = null,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include = null)
        {
            var skipCount = (pageNumber - 1) * pageSize;
            return GetSelectByFilterQuery(predicate, orderBy, include).Skip(pageSize).Take(skipCount).ToListAsync();
        }

        private IQueryable<T> GetSelectByFilterQuery(
            Expression<Func<T, bool>> predicate,
            Expression<Func<IQueryable<T>, IOrderedQueryable<T>>> orderBy,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include)
        {
            var query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include.Compile()(query);
            }

            if (orderBy != null)
            {
                return orderBy.Compile()(query);
            }

            return query;
        }

        public Task<List<T>> SelectByFilterAsync(
            Expression<Func<T, bool>> predicate = null,
            string orderBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include = null)
        {
            return GetSelectByFilterQuery(
                    predicate, 
                    orderBy, 
                    sortDirection, 
                    include)
                .ToListAsync();
        }

        public Task<List<T>> SelectByFilterAndPaginationAsync(
            int pageNumber = 1,
            int pageSize = 1,
            Expression<Func<T, bool>> predicate = null,
            string orderBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include = null)
        {
            var skipCount = (pageNumber - 1) * pageSize;
            return GetSelectByFilterQuery(
                    predicate, 
                    orderBy, 
                    sortDirection,
                    include)
                .Skip(skipCount)
                .Take(pageSize)
                .ToListAsync();
        }

        private IQueryable<T> GetSelectByFilterQuery(
            Expression<Func<T, bool>> predicate,
            string orderBy,
            SortDirection sortDirection,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include)
        {
            var query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include.Compile()(query);
            }

            if (orderBy != null)
            {
                return sortDirection == SortDirection.Ascending ? query.OrderBy(o => EF.Property<T>(o, orderBy)) : query.OrderByDescending(o => EF.Property<T>(o, orderBy));
            }

            return query;
        }

        public async Task<int> CountByFilterAsync(
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<IQueryable<T>, IIncludableQueryable<T, object>>> include = null)
        {
            var query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (include != null)
            {
                query = include.Compile()(query);
            }

            return await query.CountAsync();
        }

        public async Task AddAsync(T @object)
        {
            await _dbSet.AddAsync(@object);
        }

        public Task AddRangeAsync(params T[] @object)
        {
            return _dbSet.AddRangeAsync(@object);
        }

        public void Update(T @object)
        {
            _dbSet.Attach(@object);
            _context.Entry(@object).State = EntityState.Modified;
        }

        public void UpdateRange(params T[] objects)
        {
            foreach (var @object in objects)
            {
                _dbSet.Attach(@object);
                _context.Entry(@object).State = EntityState.Modified;
            }
        }

        public virtual void Remove(T @object)
        {
            _dbSet.Remove(@object);
        }

        public virtual void RemoveRange(IEnumerable<T> objects)
        {
            _dbSet.RemoveRange(objects);
        }
    }
}
