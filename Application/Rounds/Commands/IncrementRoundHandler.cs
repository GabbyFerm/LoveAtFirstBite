using Application.Interfaces;
using Application.Rounds.DTOs;
using AutoMapper;
using Domain.Common;
using Domain.Models;
using MediatR;

namespace Application.Rounds.Commands
{
    public class IncrementRoundHandler : IRequestHandler<IncrementRoundCommand, OperationResult<RoundDto>>
    {
        private readonly IGenericRepository<Round> _repository;
        private readonly IMapper _mapper;

        public IncrementRoundHandler(IGenericRepository<Round> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<RoundDto>> Handle(IncrementRoundCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var latestRound = _repository.AsQueryable()
                    .OrderByDescending(r => r.UpdatedAt)
                    .FirstOrDefault();

                Round round;
                if (latestRound == null)
                {
                    round = new Round { CurrentRound = 1, UpdatedAt = DateTime.UtcNow };
                    var result = await _repository.AddAsync(round, cancellationToken);
                    if (!result.IsSuccess)
                        return OperationResult<RoundDto>.Failure(result.ErrorMessage);
                    round = result.Data;
                }
                else
                {
                    latestRound.CurrentRound++;
                    latestRound.UpdatedAt = DateTime.UtcNow;
                    var result = await _repository.UpdateAsync(latestRound, cancellationToken);
                    if (!result.IsSuccess)
                        return OperationResult<RoundDto>.Failure(result.ErrorMessage);
                    round = result.Data;
                }

                var dto = _mapper.Map<RoundDto>(round);
                return OperationResult<RoundDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return OperationResult<RoundDto>.Failure($"Unexpected error: {ex.Message}");
            }
        }
    }
}
