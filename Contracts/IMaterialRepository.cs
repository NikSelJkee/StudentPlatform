using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> GetMaterialsAsync(int tagId, bool trackChanges);
        Task<IEnumerable<Material>> GetMaterialsByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        Task<Material> GetMaterialAsync(int tagId, int materialId, bool trackChanges);
        void CreateMaterial(int tagId, Material material);
        void DeleteMaterial(Material material);
    }
}
