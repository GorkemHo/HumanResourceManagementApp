using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ik_Bitirme.Domain.Entities;
using Ik_Bitirme.Domain.IRepositories;
using Ik_Bitirme.Infrastructure.Context;

namespace Ik_Bitirme.Infrastructure.Repositories
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class, IBaseEntity
    {
        private readonly AppDbContext _context;
        protected DbSet<T> _table;

        public BaseRepo(AppDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public async Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            //_context.Set<T>().Any(expression); // Bu işlem yerine constructorda ilgili tabloyu kullanabiliriz.
            return await _table.AnyAsync(expression);

        }

        public virtual async Task CreateAsync(T entity) // Virtual ile istersek metodu ezebiliriz.
        {
            await _table.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            //await _table.RemoveAsync(entity); // RemoveAsync diye bir metot yok.
            _context.Entry<T>(entity).State = EntityState.Deleted; //Entity State durumunu değiştirerek silebiliriz.
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetDefault(Expression<Func<T, bool>> expression)
        {
            return await _table.FirstOrDefaultAsync(expression);
        }

        public async Task<List<T>> GetDefaults(Expression<Func<T, bool>> expression)
        {
            return await _table.Where(expression).ToListAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<TResult> GetFilteredFirstOrDefault<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _table;

            if (where != null)
                query = query.Where(where);

            if (include != null)
                query = include(query); //IIncludableQueryable sayesinde include(query) gibi bir yazım kullanabiliriz.

            if (orderby != null)
                return await orderby(query).Select(select).FirstOrDefaultAsync(); //IOrderedQueryable sayesinde orderby(query) yazabiliyoruz.
            else
                return await query.Select(select).FirstOrDefaultAsync();

        }

        public async Task<List<TResult>> GetFilteredList<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> where, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _table;

            if (where != null)
                query = query.Where(where);

            if (include != null)
                query = include(query);

            if (orderby != null)
                return await orderby(query).Select(select).ToListAsync();
            else
                return await query.Select(select).ToListAsync();
        }
    }
}
