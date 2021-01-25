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
    public class TestRepository : RepositoryBase<Test>, ITestRepository
    {
        public TestRepository(StudentPlatformContext context)
            : base(context)
        {

        }

        public void CreateTest(int materialId, Test test)
        {
            test.MaterialId = materialId;
            Create(test);
        }

        public void DeleteTest(Test test) => Delete(test);

        public async Task<Test> GetTestAsync(int materialId, int testId, bool trackChanges) =>
            await FindByCondition(t => t.MaterialId.Equals(materialId) && t.Id.Equals(testId), trackChanges)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<Test>> GetTestsAsync(int materialId, bool trackChanges) =>
            await FindByCondition(t => t.MaterialId.Equals(materialId), trackChanges)
            .ToListAsync();

        public async Task<IEnumerable<Test>> GetTestsByIdsAsync(IEnumerable<int> ids, bool trackChanges) =>
            await FindByCondition(t => ids.Contains(t.Id), trackChanges)
                .ToListAsync();
    }
}
