using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RatingRepository : RepositoryBase<Rating>, IRatingRepository
    {
        public RatingRepository(StudentPlatformContext context)
            : base(context)
        {

        }

        public void CreateRating(int materialId, string userId, Rating rating)
        {
            rating.MaterialId = materialId;
            rating.User.Id = userId;
            Create(rating);
        }

        public void DeleteRating(Rating rating) => Delete(rating);

        public async Task<Rating> GetRatingAsync(string userId, int ratingId, bool trackChanges) =>
            await FindByCondition(r => r.User.Id.Equals(userId) && r.Id.Equals(ratingId), trackChanges)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<Rating>> GetRatingsAsync(string userId, bool trackChanges) =>
            await FindByCondition(r => r.User.Id.Equals(userId), trackChanges)
            .ToListAsync();

        public async Task<IEnumerable<Rating>> GetRatingsByIdsAsync(IEnumerable<int> ids, bool trackChanges) =>
            await FindByCondition(r => ids.Contains(r.Id), trackChanges)
            .ToListAsync();
    }
}
