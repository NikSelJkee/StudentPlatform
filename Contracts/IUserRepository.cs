using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync(int groupId, bool trackChanges);
        Task<IEnumerable<User>> GetUsersByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        Task<User> GetUserAsync(int groupId, string userId, bool trackChanges);
        void CreateUserForGroup(int groupId, User user);
        void DeleteUserForGroup(User user);
    }
}
