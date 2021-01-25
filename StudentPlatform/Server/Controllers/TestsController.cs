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
    [Route("api/tags/{tagId}/materials/{materialId}/[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public TestsController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTestsForMaterial(int tagId, int materialId)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var material = await _repository.Material.GetMaterialAsync(tagId, materialId, trackChanges: false);

            if (material == null)
                return NotFound($"Material with id: {materialId} doesn't exists in the database");

            var tests = await _repository.Test.GetTestsAsync(materialId, trackChanges: false);
            var testsDto = _mapper.Map<IEnumerable<TestDto>>(tests);

            return Ok(testsDto);
        }

        [HttpGet("collection/({ids})", Name = "GetTestCollectionForMaterial")]
        public async Task<IActionResult> GetTestCollectionForMaterial(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
                return BadRequest("Parameters ids is null");

            var testEntities = await _repository.Test.GetTestsByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != testEntities.Count())
                return BadRequest("Some ids are not valid in a collection");

            var testsToReturn = _mapper.Map<IEnumerable<TestDto>>(testEntities);

            return Ok(testsToReturn);
        }

        [HttpGet("{testId}", Name = "GetTestForMaterial")]
        public async Task<IActionResult> GetTestForMaterial(int tagId, int materialId, int testId)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var material = await _repository.Material.GetMaterialAsync(tagId, materialId, trackChanges: false);

            if (material == null)
                return NotFound($"Material with id: {materialId} doesn't exists in the database");

            var test = await _repository.Test.GetTestAsync(materialId, testId, trackChanges: false);

            if (test == null)
                return NotFound($"Test with id: {testId} doesn't exists in the database");

            var testDto = _mapper.Map<TestDto>(test);

            return Ok(testDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestForMaterial(int tagId, int materialId,
            [FromBody] TestForCreationDto test)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var material = await _repository.Material.GetMaterialAsync(tagId, materialId, trackChanges: false);

            if (material == null)
                return NotFound($"Material with id: {materialId} doesn't exists in the database");

            var testEntity = _mapper.Map<Test>(test);

            _repository.Test.CreateTest(materialId, testEntity);
            await _repository.SaveAsync();

            var testToReturn = _mapper.Map<TestDto>(testEntity);

            return CreatedAtRoute("GetTestForMaterial",
                new { tagId = tagId, materialId = materialId, testId = testToReturn.Id }, testToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateTestCollectionForMaterial(int tagId, int materialId,
            [FromBody] IEnumerable<TestForCreationDto> testCollection)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var material = await _repository.Material.GetMaterialAsync(tagId, materialId, trackChanges: false);

            if (material == null)
                return NotFound($"Material with id: {materialId} doesn't exists in the database");

            var testEntities = _mapper.Map<IEnumerable<Test>>(testCollection);
            foreach (var test in testEntities)
            {
                _repository.Test.CreateTest(materialId, test);
            }

            await _repository.SaveAsync();

            var testCollectionToReturn = _mapper.Map<IEnumerable<TestDto>>(testEntities);
            var ids = string.Join(",", testCollectionToReturn.Select(t => t.Id));

            return CreatedAtRoute("GetTestCollectionForMaterial", new { ids = ids }, testCollectionToReturn);
        }

        [HttpDelete("{testId}")]
        public async Task<IActionResult> DeleteTestForMaterial(int tagId, int materialId, int testId)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var material = await _repository.Material.GetMaterialAsync(tagId, materialId, trackChanges: false);

            if (material == null)
                return NotFound($"Material with id: {materialId} doesn't exists in the database");

            var test = await _repository.Test.GetTestAsync(materialId, testId, trackChanges: false);

            if (test == null)
                return NotFound($"Test with id: {testId} doesn't exists in the database");

            _repository.Test.DeleteTest(test);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{testId}")]
        public async Task<IActionResult> UpdateTestForMaterial(int tagId, int materialId, int testId, 
            [FromBody]TestForUpdateDto test)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var material = await _repository.Material.GetMaterialAsync(tagId, materialId, trackChanges: false);

            if (material == null)
                return NotFound($"Material with id: {materialId} doesn't exists in the database");

            var testEntity = await _repository.Test.GetTestAsync(materialId, testId, trackChanges: true);

            if (testEntity == null)
                return NotFound($"Test with id: {testId} doesn't exists in the database");

            _mapper.Map(test, testEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{testId}")]
        public async Task<IActionResult> PartiallyUpdateTestForMaterial(int tagId, int materialId, int testId, 
            [FromBody]JsonPatchDocument<TestForUpdateDto> patchDocument)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var material = await _repository.Material.GetMaterialAsync(tagId, materialId, trackChanges: false);

            if (material == null)
                return NotFound($"Material with id: {materialId} doesn't exists in the database");

            var testEntity = await _repository.Test.GetTestAsync(materialId, testId, trackChanges: true);

            if (testEntity == null)
                return NotFound($"Test with id: {testId} doesn't exists in the database");

            var testToPatch = _mapper.Map<TestForUpdateDto>(testEntity);
            patchDocument.ApplyTo(testToPatch);

            TryValidateModel(testToPatch);

            _mapper.Map(testToPatch, testEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
