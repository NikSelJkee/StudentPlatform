using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetTagsAsync(bool trackChanges);
        Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        Task<Tag> GetTagAsync(int tagId, bool trackChanges);
        void CreateTag(Tag tag);
        void DeleteTag(Tag tag);
    }
}
