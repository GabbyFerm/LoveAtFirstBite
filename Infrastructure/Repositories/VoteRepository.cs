using Application.Interfaces;
using Domain.Common;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private readonly LoveAtFirstBiteDbContext _dbContext;

        public VoteRepository(LoveAtFirstBiteDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }


        public async Task<OperationResult<IEnumerable<Vote>>> GetAllVotesAsync()
        {
            try
            {
                var votes = await _dbContext.Votes.ToListAsync();
                return OperationResult<IEnumerable<Vote>>.Success(votes);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<Vote>>.Failure(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }

}
