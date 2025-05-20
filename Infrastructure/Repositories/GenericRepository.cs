using Application.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<OperationResult<T>> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                // This captures the deeper cause
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return OperationResult<T>.Failure(errorMessage);
            }
        }


        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return OperationResult<bool>.Failure("Entity not found");
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }

        public async Task<OperationResult<IEnumerable<T>>> GetAllAsync()
        {
            var list = await _dbSet.ToListAsync();
            return OperationResult<IEnumerable<T>>.Success(list);
        }

        public async Task<OperationResult<T>> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity != null
                ? OperationResult<T>.Success(entity)
                : OperationResult<T>.Failure("Entity not found");
        }

        public async Task<OperationResult<T>> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return OperationResult<T>.Success(entity);
        }
    }
}
