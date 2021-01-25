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
    [Route("api/groups/{groupId}/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UsersController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersForGroup(int groupId)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var users = await _repository.User.GetUsersAsync(groupId, trackChanges: false);

            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

            return Ok(usersDto);
        }

        [HttpGet("collection/({ids})", Name = "GetUsersForGroupCollection")]
        public async Task<IActionResult> GetUsersForGroupCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
                return BadRequest("Parameters ids is null");

            var userEntities = await _repository.User.GetUsersByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != userEntities.Count())
                return BadRequest("Some ids are not valid in a collection");

            var usersToReturn = _mapper.Map<IEnumerable<UserDto>>(userEntities);

            return Ok(usersToReturn);
        }

        [HttpGet("{userId}", Name = "GetUserForGroup")]
        public async Task<IActionResult> GetUserForGroup(int groupId, string userId)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var user = await _repository.User.GetUserAsync(groupId, userId, trackChanges: false);

            if (user == null)
                return NotFound($"User with id: {userId} doesn't exists in the database");

            var userDto = _mapper.Map<UserDto>(user);

            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserForGroup(int groupId, [FromBody]UserForCreationDto user)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var userEntity = _mapper.Map<User>(user);

            _repository.User.CreateUserForGroup(groupId, userEntity);
            await _repository.SaveAsync();

            var userToReturn = _mapper.Map<UserDto>(userEntity);

            return CreatedAtRoute("GetUserForGroup", new { groupId, userId = userToReturn.Id }, userToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateUserCollectionForGroup(int groupId, 
            [FromBody]IEnumerable<UserForCreationDto> userCollection)
        {
            if (userCollection == null)
                return BadRequest("User collection is null");

            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var userEntities = _mapper.Map<IEnumerable<User>>(userCollection);
            foreach (var user in userEntities)
            {
                _repository.User.CreateUserForGroup(groupId, user);
            }

            await _repository.SaveAsync();

            var userCollectionToReturn = _mapper.Map<IEnumerable<UserDto>>(userEntities);
            var ids = string.Join(",", userCollectionToReturn.Select(u => u.Id));

            return CreatedAtRoute("GetUsersForGroupCollection", new { groupId, ids = ids }, userCollectionToReturn);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserForGroup(int groupId, string userId)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var user = await _repository.User.GetUserAsync(groupId, userId, trackChanges: false);

            if (user == null)
                return NotFound($"User with id: {userId} doesn't exists in the database");

            _repository.User.DeleteUserForGroup(user);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserForGroup(int groupId, string userId, 
            [FromBody]UserForUpdateDto user)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var userEntity = await _repository.User.GetUserAsync(groupId, userId, trackChanges: true);

            if (user == null)
                return NotFound($"User with id: {userId} doesn't exists in the database");

            _mapper.Map(user, userEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> PartiallyUpdateUserForGroup(int groupId, string userId,
            [FromBody] JsonPatchDocument<UserForUpdateDto> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest("patchDocument object is null");

            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var userEntity = await _repository.User.GetUserAsync(groupId, userId, trackChanges: true);

            if (userEntity == null)
                return NotFound($"User with id: {userId} doesn't exists in the database");

            var userToPatch = _mapper.Map<UserForUpdateDto>(userEntity);
            patchDocument.ApplyTo(userToPatch);

            TryValidateModel(userToPatch);

            _mapper.Map(userToPatch, userEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
