using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IGroupRepository Group { get; }
        IUserRepository User { get; }
        ITagRepository Tag { get; }
        IMaterialRepository Material { get; }
        ITestRepository Test { get; }
        IRatingRepository Rating { get; }
        Task SaveAsync();
    }
}
