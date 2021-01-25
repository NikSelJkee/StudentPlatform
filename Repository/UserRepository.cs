using Contracts;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly UserManager<User> _userManger;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(StudentPlatformContext context, 
            UserManager<User> userManger, SignInManager<User> signInManager) 
            : base(context)
        {
            _userManger = userManger;
            _signInManager = signInManager;
        }

        public void CreateUserForGroup(int groupId, User user)
        {
            user.GroupId = groupId;

            Create(user);
        }

        public void DeleteUserForGroup(User user) => Delete(user);


        public async Task<User> GetUserAsync(int groupId, string userId, bool trackChanges) =>
            await FindByCondition(u => u.Group.Id.Equals(groupId) && u.Id.Equals(userId), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<User>> GetUsersAsync(int groupId, bool trackChanges) =>
            await FindByCondition(u => u.Group.Id.Equals(groupId), trackChanges)
            .OrderBy(u => u.LastName)
            .ToListAsync();

        public async Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<int> ids, bool trackChanges) =>
            await FindByCondition(u => ids.Contains(int.Parse(u.Id)), trackChanges)
            .OrderBy(u => u.LastName)
            .ToListAsync();
    }
}
