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
    [Route("api/groups/{groupId}/users/{userId}/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public RatingsController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRatingsForUser(int groupId, string userId)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var user = await _repository.User.GetUserAsync(groupId, userId, trackChanges: false);

            if (user == null)
                return NotFound($"User with id: {userId} doesn't exists in the database");

            var ratings = await _repository.Rating.GetRatingsAsync(userId, trackChanges: false);
            //var ratingsDto = _mapper.Map<IEnumerable<RatingDto>>(ratings);

            return Ok(ratings);
        }

        [HttpGet("collection/({ids})", Name = "GetRatingCollectionForUser")]
        public async Task<IActionResult> GetRatingCollectionForUser(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
                return BadRequest("Parameters ids is null");

            var ratingEntities = await _repository.Rating.GetRatingsByIdsAsync(ids, trackChanges: false);

            if (ids.Count() != ratingEntities.Count())
                return BadRequest("Some ids are not valid in a collection");

            var ratingToReturn = _mapper.Map<IEnumerable<RatingDto>>(ratingEntities);

            return Ok(ratingToReturn);
        }

        [HttpGet("{ratingId}", Name = "GetRatingForUser")]
        public async Task<IActionResult> GetRatingForUser(int groupId, string userId, int ratingId)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var user = await _repository.User.GetUserAsync(groupId, userId, trackChanges: false);

            if (user == null)
                return NotFound($"User with id: {userId} doesn't exists in the database");

            var rating = await _repository.Rating.GetRatingAsync(userId, ratingId, trackChanges: false);

            if (rating == null)
                return NotFound($"Rating with id: {ratingId} doesn't exists in the database");

            var ratingDto = _mapper.Map<RatingDto>(rating);

            return Ok(ratingDto);
        }

        [HttpPost("{materialId}")]
        public async Task<IActionResult> CreateRatingForUser(int materialId, int groupId, string userId, 
            [FromBody]RatingForCreationDto rating)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var user = await _repository.User.GetUserAsync(groupId, userId, trackChanges: false);

            if (user == null)
                return NotFound($"User with id: {userId} doesn't exists in the database");

            var ratingEntity = _mapper.Map<Rating>(rating);

            _repository.Rating.CreateRating(materialId, userId, ratingEntity);
            await _repository.SaveAsync();

            var ratingToReturn = _mapper.Map<RatingDto>(ratingEntity);

            return CreatedAtRoute("GetRatingForUser", new { groupId, userId, ratingId = ratingToReturn.Id }, ratingToReturn);
        }

        [HttpPost("collection/{materialId}")]
        public async Task<IActionResult> CreateRatingCollectionForUser(int materialId, int groupId, string userId, 
            [FromBody]IEnumerable<RatingForCreationDto> ratingCollection)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var user = await _repository.User.GetUserAsync(groupId, userId, trackChanges: false);

            if (user == null)
                return NotFound($"User with id: {userId} doesn't exists in the database");

            var ratingEntities = _mapper.Map<IEnumerable<Rating>>(ratingCollection);
            foreach (var rating in ratingEntities)
            {
                _repository.Rating.CreateRating(materialId, userId, rating);
            }

            await _repository.SaveAsync();

            var ratingCollectionToReturn = _mapper.Map<IEnumerable<RatingDto>>(ratingEntities);
            var ids = string.Join(",", ratingCollectionToReturn.Select(r => r.Id));

            return CreatedAtRoute("GetRatingCollectionForUser", new { ids = ids }, ratingCollectionToReturn);
        }

        [HttpDelete("{ratingId}")]
        public async Task<IActionResult> DeleteRatingForUser(int groupId, string userId, int ratingId)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var user = await _repository.User.GetUserAsync(groupId, userId, trackChanges: false);

            if (user == null)
                return NotFound($"User with id: {userId} doesn't exists in the database");

            var rating = await _repository.Rating.GetRatingAsync(userId, ratingId, trackChanges: false);

            if (rating == null)
                return NotFound($"Rating with id: {ratingId} doesn't exists in the database");

            _repository.Rating.DeleteRating(rating);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{ratingId}")]
        public async Task<IActionResult> UpdateRatingForUser(int groupId, string userId, int ratingId, 
            [FromBody]RatingForUpdateDto rating)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var user = await _repository.User.GetUserAsync(groupId, userId, trackChanges: false);

            if (user == null)
                return NotFound($"User with id: {userId} doesn't exists in the database");

            var ratingEntity = await _repository.Rating.GetRatingAsync(userId, ratingId, trackChanges: true);

            if (ratingEntity == null)
                return NotFound($"Rating with id: {ratingId} doesn't exists in the database");

            _mapper.Map(rating, ratingEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{ratingId}")]
        public async Task<IActionResult> PartiallyUpdateRatingForUser(int groupId, string userId, int ratingId, 
            [FromBody]JsonPatchDocument<RatingForUpdateDto> patchDocument)
        {
            var group = await _repository.Group.GetGroupAsync(groupId, trackChanges: false);

            if (group == null)
                return NotFound($"Group with id: {groupId} doesn't exists in the database");

            var user = await _repository.User.GetUserAsync(groupId, userId, trackChanges: false);

            if (user == null)
                return NotFound($"User with id: {userId} doesn't exists in the database");

            var ratingEntity = await _repository.Rating.GetRatingAsync(userId, ratingId, trackChanges: true);

            if (ratingEntity == null)
                return NotFound($"Rating with id: {ratingId} doesn't exists in the database");

            var ratingToPatch = _mapper.Map<RatingForUpdateDto>(ratingEntity);
            patchDocument.ApplyTo(ratingToPatch);

            TryValidateModel(ratingToPatch);

            _mapper.Map(ratingToPatch, ratingEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}
