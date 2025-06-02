using Domain.Common;
using Domain.Models;

namespace Application.Interfaces
{
    public interface IVoteRepository
    {
        Task<OperationResult<IEnumerable<Vote>>> GetAllVotesAsync();
    }

}
