using Application.Interfaces;
using Domain.Common;
using Domain.Models;
using MediatR;

namespace Application.Votes.Commands.ResetVotes
{
    public class ResetVotesHandler : IRequestHandler<ResetVotesCommand, OperationResult<bool>>
    {
        private readonly IGenericRepository<Vote> _voteRepository;

        public ResetVotesHandler(IGenericRepository<Vote> voteRepository)
        {
            _voteRepository = voteRepository;
        }

        public async Task<OperationResult<bool>> Handle(ResetVotesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _voteRepository.DeleteAllAsync();

                if (!result.IsSuccess)
                    return OperationResult<bool>.Failure(result.ErrorMessage!);

                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Unexpected error: {ex.Message}");
            }
        }
    }
}
