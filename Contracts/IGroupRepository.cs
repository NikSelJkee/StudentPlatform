using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetGroupsAsync(bool trackChanges);
        Task<IEnumerable<Group>> GetGroupsByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        Task<Group> GetGroupAsync(int groupId, bool trackChanges);
        void CreateGroup(Group group);
        void DeleteGroup(Group group);
    }
}
