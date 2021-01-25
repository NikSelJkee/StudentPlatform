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
    public class GroupsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GroupsController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            var groups = await _repository.Group.GetGroupsAsync(trackChanges: false);
            var groupsDto = _mapper.Map<IEnumerable<GroupDto>>(groups);

            return Ok(groupsDto);
        }

        [HttpGet("collection/({ids})", Name = "GetGroupCollection")]
        public async Task<IActionResult> GetGroupCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
                return BadRequest("Parameters ids is null");

            var groupEntities = await _repository.Group.GetGroupsByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != groupEntities.Count())
                return NotFound("Some ids are not valid in a collection");

            var groupsToReturn = _mapper.Map<IEnumerable<GroupDto>>(groupEntities);

            return Ok(groupsToReturn);
        }

        [HttpGet("{groupId}", Name = "GetGroup")]
        public async Task<IActionResult> GetGroup(int groupId)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var groupDto = _mapper.Map<GroupDto>(group);

            return Ok(groupDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody]GroupForCreationDto group)
        {
            var groupEntity = _mapper.Map<Group>(group);

            _repository.Group.CreateGroup(groupEntity);
            await _repository.SaveAsync();

            var groupToReturn = _mapper.Map<GroupDto>(groupEntity);

            return CreatedAtRoute("GetGroup", new { groupId = groupToReturn.Id }, groupToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateGroupCollection(
            [FromBody]IEnumerable<GroupForCreationDto> groupCollection)
        {
            if (groupCollection == null)
                return BadRequest("Group collection is null");

            var groupEntities = _mapper.Map<IEnumerable<Group>>(groupCollection);
            foreach (var group in groupEntities)
            {
                _repository.Group.CreateGroup(group);
            }

            await _repository.SaveAsync();

            var groupCollectionToReturn = _mapper.Map<IEnumerable<GroupDto>>(groupEntities);
            var ids = string.Join(",", groupCollectionToReturn.Select(c => c.Id));

            return CreatedAtRoute("GetGroupCollection", new { ids = ids }, groupCollectionToReturn);
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            _repository.Group.DeleteGroup(group);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{groupId}")]
        public async Task<IActionResult> UpdateGroup(int groupId, [FromBody]GroupForUpdateDto group)
        {
            var groupEntity = await _repository.Group.GetGroupAsync(groupId, trackChanges: true);

            if (groupEntity == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            _mapper.Map(group, groupEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{groupId}")]
        public async Task<IActionResult> PartiallyUpdateGroup(int groupId, 
            [FromBody]JsonPatchDocument<GroupForUpdateDto> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest("patchDocument object is null");

            var groupEntity = await _repository.Group.GetGroupAsync(groupId, trackChanges: true);

            if (groupEntity == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var groupToPatch = _mapper.Map<GroupForUpdateDto>(groupEntity);
            patchDocument.ApplyTo(groupToPatch);

            TryValidateModel(groupToPatch);

            _mapper.Map(groupToPatch, groupEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
