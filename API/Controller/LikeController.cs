using API.Dtos;
using API.Entity;
using API.Extensions;
using API.Helper;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{
    [Authorize]

    public class LikeController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public LikeController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username){
            var sourceUserId = User.GetUserId();
            var likedUser = await _uow.UserRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _uow.LikeRepository.GetUserWithLikes(sourceUserId);

            if (likedUser == null)
            {
                return NotFound();
            }
            if (sourceUser.UserName == username)
            {
                return BadRequest("You cannot like yourself");
            }
            var userLike = await _uow.LikeRepository.GetUserLike(sourceUserId,likedUser.Id);
            if (userLike != null)
            {
                return BadRequest("You already like this user");
            }

            userLike = new UserLike{
                SourceUserId = sourceUserId,
                TargetUserId = likedUser.Id,
            };
            sourceUser.LikedUsers.Add(userLike);
            if (await _uow.Complete())
            {
                return Ok();
            }
            return BadRequest("Failed to like user");

        }

            [HttpGet]
            public async Task<ActionResult<PagedList<LikeDto>>> GetUsersLike([FromQuery]LikeParams likesParams){

                likesParams.UserId = User.GetUserId();
                var users = await _uow.LikeRepository.GetUsersLike(likesParams);

                Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, 
                users.PageSize, users.TotalCount, users.TotalPages));
                return Ok(users);

            } 
    }
}