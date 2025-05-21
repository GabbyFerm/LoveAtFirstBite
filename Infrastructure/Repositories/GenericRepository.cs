using Application.Interfaces;
using Domain.Common;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly LoveAtFirstBiteDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(LoveAtFirstBiteDbContext context)
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

                return OperationResult<T>.Failure(GetExceptionMessage(ex));
            }
        }


        public async Task<OperationResult<bool>> DeleteByIdAsync(int id)
        {
            try
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
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure(GetExceptionMessage(ex));
            }


        }

        public async Task<OperationResult<IEnumerable<T>>> GetAllAsync()
        {
            try
            {

                var list = await _dbSet.ToListAsync();
                return OperationResult<IEnumerable<T>>.Success(list);

            }
            catch (Exception ex)

            {

                return OperationResult<IEnumerable<T>>.Failure(GetExceptionMessage(ex));


            }

        }


        public async Task<OperationResult<T>> GetByIdAsync(int id)
        {
            try
            {

                var entity = await _dbSet.FindAsync(id);
                return entity != null
                    ? OperationResult<T>.Success(entity)
                    : OperationResult<T>.Failure("Entity not found");

            }
            catch (Exception ex)
            {
                return OperationResult<T>.Failure(GetExceptionMessage(ex));
            }

        }

        public async Task<OperationResult<T>> UpdateAsync(T entity)
        {

            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Success(entity);
            }
            catch (Exception ex)
            {

                return OperationResult<T>.Failure(GetExceptionMessage(ex));

            }

        }

        private static string GetExceptionMessage(Exception ex)
        {
            return ex.InnerException?.Message ?? ex.Message;
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}
