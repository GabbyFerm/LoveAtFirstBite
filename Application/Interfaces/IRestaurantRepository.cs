using Application.Authorize.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRestaurantRepository
    {
        /// <summary>
        /// Returns today’s vote tally for all restaurants (including zero-vote ones),
        /// with vote counts computed in CET, sorted descending,
        /// and leaders flagged.
        /// </summary>
        Task<List<RestaurantVoteDto>> GetTodayVoteTallyAsync();
    }
}