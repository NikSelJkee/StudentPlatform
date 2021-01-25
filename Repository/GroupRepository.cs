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
    public class GroupRepository : RepositoryBase<Group>, IGroupRepository
    {
        public GroupRepository(StudentPlatformContext context) 
            : base(context)
        {

        }

        public void CreateGroup(Group group) => Create(group);

        public void DeleteGroup(Group group) => Delete(group);

        public async Task<Group> GetGroupAsync(int id, bool trackChanges) =>
            await FindByCondition(g => g.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Group>> GetGroupsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
            .OrderBy(g => g.Name)
            .ToListAsync();

        public async Task<IEnumerable<Group>> GetGroupsByIdsAsync(IEnumerable<int> ids, bool trackChanges) =>
            await FindByCondition(g => ids.Contains(g.Id), trackChanges)
            .OrderBy(g => g.Name)
            .ToListAsync();
    }
}
