using Application.Interfaces;
using Application.Rounds.DTOs;
using AutoMapper;
using Domain.Common;
using Domain.Models;
using MediatR;

namespace Application.Rounds.Queries
{
    public class GetCurrentRoundHandler : IRequestHandler<GetCurrentRoundQuery, OperationResult<RoundDto>>
    {
        private readonly IGenericRepository<Round> _repository;
        private readonly IMapper _mapper;

        public GetCurrentRoundHandler(IGenericRepository<Round> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<RoundDto>> Handle(GetCurrentRoundQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var rounds = await _repository.GetAllAsync();

                if (!rounds.IsSuccess || rounds.Data is null || !rounds.Data.Any())
                    return OperationResult<RoundDto>.Failure("No round found.");

                var latestRound = rounds.Data.OrderByDescending(r => r.UpdatedAt).First();
                var dto = _mapper.Map<RoundDto>(latestRound);

                return OperationResult<RoundDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return OperationResult<RoundDto>.Failure($"Unexpected error: {ex.Message}");
            }
        }
    }
}
