using Application.Interfaces;
using Application.Votes.Dtos;
using Application.Votes.Queries;
using Domain.Common;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetUserVoteStatusHandler : IRequestHandler<GetUserVoteStatusQuery, OperationResult<UserVoteStatusDto>>
{
    private readonly IGenericRepository<Vote> _voteRepository;

    public GetUserVoteStatusHandler(IGenericRepository<Vote> voteRepository)
    {
        _voteRepository = voteRepository;
    }

    public async Task<OperationResult<UserVoteStatusDto>> Handle(GetUserVoteStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var vote = await _voteRepository
                .AsQueryable()
                .FirstOrDefaultAsync(v => v.UserId == request.UserId && v.Round == request.Round, cancellationToken);

            var dto = new UserVoteStatusDto
            {
                HasVoted = vote != null,
                VotedRestaurantId = vote?.RestaurantId
            };

            return OperationResult<UserVoteStatusDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return OperationResult<UserVoteStatusDto>.Failure(ex.Message);
        }
    }
}
