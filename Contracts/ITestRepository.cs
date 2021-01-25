using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITestRepository
    {
        Task<IEnumerable<Test>> GetTestsAsync(int materialId, bool trackChanges);
        Task<IEnumerable<Test>> GetTestsByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        Task<Test> GetTestAsync(int materialId, int testId, bool trackChanges);
        void CreateTest(int materialId, Test test);
        void DeleteTest(Test test);
    }
}
