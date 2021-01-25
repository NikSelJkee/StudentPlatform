using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetRatingsAsync(string userId, bool trackChanges);
        Task<IEnumerable<Rating>> GetRatingsByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        Task<Rating> GetRatingAsync(string userId, int ratingId, bool trackChanges);
        void CreateRating(int materialId, string userId, Rating rating);
        void DeleteRating(Rating rating);
    }
}
