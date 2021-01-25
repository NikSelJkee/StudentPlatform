using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using StudentPlatform.Server.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentPlatform.Server.Controllers
{
    [ApiController]
    [Route("api/tags/{tagId}/[controller]")]
    public class MaterialsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public MaterialsController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMaterialsForTag(int tagId)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var materials = await _repository.Material.GetMaterialsAsync(tagId, trackChanges: false);
            var materialsDto = _mapper.Map<IEnumerable<MaterialDto>>(materials);

            return Ok(materialsDto);
        }

        [HttpGet("collection/{materialId}", Name = "GetMaterialCollectionForTag")]
        public async Task<IActionResult> GetMaterialsForTagCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
                return BadRequest("Parameters ids is null");

            var materialEntities = await _repository.Material.GetMaterialsByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != materialEntities.Count())
                return NotFound("Some ids are not valid in a collection");

            var materialsToReturn = _mapper.Map<IEnumerable<MaterialDto>>(materialEntities);

            return Ok(materialsToReturn);
        }

        [HttpGet("{materialId}", Name = "GetMaterialForTag")]
        public async Task<IActionResult> GetMaterialForTag(int tagId, int materialId)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var material = await _repository.Material.GetMaterialAsync(tagId, materialId, trackChanges: false);

            if (material == null)
                return NotFound($"Material with id: {materialId} doesn't exists in the database");

            var materialDto = _mapper.Map<MaterialDto>(material);

            return Ok(materialDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterialForTag(int tagId, [FromBody]MaterialForCreationDto material)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var materialEntity = _mapper.Map<Material>(material);

            _repository.Material.CreateMaterial(tagId, materialEntity);
            await _repository.SaveAsync();

            return CreatedAtRoute("GetMaterialForTag", new { tagId, materialId = materialEntity.Id }, materialEntity);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateMaterialCollectionForTag(int tagId,
            [FromBody]IEnumerable<MaterialForCreationDto> materialCollection)
        {
            if (materialCollection == null)
                return BadRequest("Material collection is null");

            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var materialEntities = _mapper.Map<IEnumerable<Material>>(materialCollection);
            foreach (var material in materialEntities)
            {
                _repository.Material.CreateMaterial(tagId, material);
            }

            await _repository.SaveAsync();

            var materialCollectionToReturn = _mapper.Map<IEnumerable<MaterialDto>>(materialEntities);
            var ids = string.Join(",", materialCollectionToReturn.Select(m => m.Id));

            return CreatedAtRoute("GetMaterialCollectionForTag", new { ids = ids }, materialCollectionToReturn);
        }

        [HttpDelete("{materialId}")]
        public async Task<IActionResult> DeleteMaterialForTag(int tagId, int materialId)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exist in the database");

            var material = await _repository.Material.GetMaterialAsync(tagId, materialId, trackChanges: false);

            if (material == null)
                return NotFound($"Material with id: {materialId} doesn't exists in the database");

            _repository.Material.DeleteMaterial(material);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{materialId}")]
        public async Task<IActionResult> UpdateMaterialForTag(int tagId, int materialId, 
            [FromBody]MaterialForUpdateDto material)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exist in the database");

            var materialEntity = await _repository.Material.GetMaterialAsync(tagId, materialId, trackChanges: true);

            if (materialEntity == null)
                return NotFound($"Material with id: {materialId} doesn't exists in the database");

            _mapper.Map(material, materialEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{materialId}")]
        public async Task<IActionResult> PartiallyUpdateMaterialForTag(int tagId, int materialId, 
            [FromBody]JsonPatchDocument<MaterialForUpdateDto> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest("patchDocument object is null");

            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exist in the database");

            var materialEntity = await _repository.Material.GetMaterialAsync(tagId, materialId, trackChanges: true);

            if (materialEntity == null)
                return NotFound($"Material with id: {materialId} doesn't exists in the database");

            var materialToPatch = _mapper.Map<MaterialForUpdateDto>(materialEntity);
            patchDocument.ApplyTo(materialToPatch);

            TryValidateModel(materialToPatch);

            _mapper.Map(materialToPatch, materialEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
