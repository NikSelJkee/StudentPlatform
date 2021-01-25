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
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(StudentPlatformContext context) :
            base (context)
        {

        }

        public void CreateTag(Tag tag) => Create(tag);

        public void DeleteTag(Tag tag) => Delete(tag);

        public async Task<Tag> GetTagAsync(int tagId, bool trackChanges) =>
            await FindByCondition(t => t.Id.Equals(tagId), trackChanges)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<Tag>> GetTagsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(t => t.Name)
            .ToListAsync();

        public async Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> ids, bool trackChanges) =>
            await FindByCondition(t => ids.Contains(t.Id), trackChanges)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }
}
