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
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public TagsController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tags = await _repository.Tag.GetTagsAsync(trackChanges: false);
            var tagsDto = _mapper.Map<IEnumerable<TagDto>>(tags);

            return Ok(tagsDto);
        }

        [HttpGet("collection/({ids})", Name = "GetTagCollection")]
        public async Task<IActionResult> GetTagCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
                return BadRequest("Parameters ids is null");

            var tagEntities = await _repository.Tag.GetTagsByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != tagEntities.Count())
                return NotFound("Some ids are not valid in a collection");

            var tagsToReturn = _mapper.Map<IEnumerable<TagDto>>(tagEntities);

            return Ok(tagsToReturn);
        }

        [HttpGet("{tagId}", Name = "GetTag")]
        public async Task<IActionResult> GetTag(int tagId)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var tagDto = _mapper.Map<TagDto>(tag);

            return Ok(tagDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody]TagForCreationDto tag)
        {
            var tagEntity = _mapper.Map<Tag>(tag);

            _repository.Tag.CreateTag(tagEntity);
            await _repository.SaveAsync();

            var tagToReturn = _mapper.Map<TagDto>(tagEntity);

            return Ok(tagToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateTagCollection(
            [FromBody]IEnumerable<TagForCreationDto> tagCollection)
        {
            if (tagCollection == null)
                return BadRequest("Tag collection is null");

            var tagEntities = _mapper.Map<IEnumerable<Tag>>(tagCollection);
            foreach (var tag in tagEntities)
            {
                _repository.Tag.CreateTag(tag);
            }

            await _repository.SaveAsync();

            var tagCollectionToReturn = _mapper.Map<IEnumerable<TagDto>>(tagEntities);
            var ids = string.Join(",", tagCollectionToReturn.Select(t => t.Id));

            return CreatedAtRoute("GetTagCollection", new { ids = ids }, tagCollectionToReturn);
        }

        [HttpDelete("{tagId}")]
        public async Task<IActionResult> DeleteTag(int tagId)
        {
            var tag = await _repository.Tag.GetTagAsync(tagId, trackChanges: false);

            if (tag == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            _repository.Tag.DeleteTag(tag);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{tagId}")]
        public async Task<IActionResult> UpdateTag(int tagId, [FromBody]TagForUpdateDto tag)
        {
            var tagEntity = await _repository.Tag.GetTagAsync(tagId, trackChanges: true);

            if (tagEntity == null)
                return NotFound($"Tag with id: {tagEntity} doesn't exists in the database");

            _mapper.Map(tag, tagEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{tagId}")]
        public async Task<IActionResult> PartiallyUpdateTag(int tagId, 
            [FromBody]JsonPatchDocument<TagForUpdateDto> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest("patchDocument object is null");

            var tagEntity = await _repository.Tag.GetTagAsync(tagId, trackChanges: true);

            if (tagEntity == null)
                return NotFound($"Tag with id: {tagId} doesn't exists in the database");

            var tagToPatch = _mapper.Map<TagForUpdateDto>(tagEntity);
            patchDocument.ApplyTo(tagToPatch);

            TryValidateModel(tagToPatch);

            _mapper.Map(tagToPatch, tagEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
