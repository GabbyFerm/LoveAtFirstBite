using Application.Interfaces;
using Domain.Common;
using Domain.Models;
using MediatR;

namespace Application.Rounds.Commands
{
    public class ResetRoundHandler : IRequestHandler<ResetRoundCommand, OperationResult<int>>
    {
        private readonly IGenericRepository<Round> _repository;

        public ResetRoundHandler(IGenericRepository<Round> repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<int>> Handle(ResetRoundCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var current = _repository.AsQueryable().FirstOrDefault();
                if (current == null)
                    return OperationResult<int>.Failure("No round found in database.");

                current.CurrentRound = 1;
                current.UpdatedAt = DateTime.UtcNow;

                var result = await _repository.UpdateAsync(current, cancellationToken);
                return result.IsSuccess
                    ? OperationResult<int>.Success(current.CurrentRound)
                    : OperationResult<int>.Failure(result.ErrorMessage ?? "Failed to reset round.");
            }
            catch (Exception ex)
            {
                return OperationResult<int>.Failure($"Unexpected error: {ex.Message}");
            }
        }
    }
}
