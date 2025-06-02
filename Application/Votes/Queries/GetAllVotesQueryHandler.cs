using Application.Interfaces;
using Application.Votes.Dtos;
using AutoMapper;
using Domain.Common;
using MediatR;

namespace Application.Votes.Queries
{
    public class GetAllVotesQueryHandler : IRequestHandler<GetAllVotesQuery, OperationResult<IEnumerable<VoteRecordDto>>>
    {
        private readonly IVoteRepository _repository;
        private readonly IMapper _mapper;

        public GetAllVotesQueryHandler(IVoteRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<IEnumerable<VoteRecordDto>>> Handle(GetAllVotesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.GetAllVotesAsync();

                if (!result.IsSuccess)
                    return OperationResult<IEnumerable<VoteRecordDto>>.Failure(result.ErrorMessage!);

                var mapped = _mapper.Map<IEnumerable<VoteRecordDto>>(result.Data);

                return OperationResult<IEnumerable<VoteRecordDto>>.Success(mapped);
            }
            catch (Exception ex)
            {
                return OperationResult<IEnumerable<VoteRecordDto>>.Failure($"Error fetching votes: {ex.Message}");
            }
        }
    }

}