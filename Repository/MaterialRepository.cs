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
    public class MaterialRepository : RepositoryBase<Material>, IMaterialRepository
    {
        public MaterialRepository(StudentPlatformContext context) 
            : base(context)
        {

        }

        public void CreateMaterial(int tagId, Material material)
        {
            material.TagId = tagId;
            Create(material);
        }

        public void DeleteMaterial(Material material) => Delete(material);

        public async Task<Material> GetMaterialAsync(int tagId, int materialId, bool trackChanges) =>
            await FindByCondition(m => m.TagId.Equals(tagId) && m.Id.Equals(materialId), trackChanges)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<Material>> GetMaterialsAsync(int tagId, bool trackChanges) =>
            await FindByCondition(m => m.TagId.Equals(tagId), trackChanges)
                .OrderBy(m => m.Name)
                .ToListAsync();

        public async Task<IEnumerable<Material>> GetMaterialsByIdsAsync(IEnumerable<int> ids, bool trackChanges) =>
            await FindByCondition(m => ids.Contains(m.Id), trackChanges)
                .OrderBy(m => m.Name)
                .ToListAsync();
    }
}
