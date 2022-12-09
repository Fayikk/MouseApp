using API.Data;
using API.Dtos;
using API.Entity;
using API.Helper;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikeRepository
    {
        
        Task<UserLike> GetUserLike(int sourceUserId , int targetUserId);
        Task<BusinessUser> GetUserWithLikes(int userId);
        Task<PagedList<LikeDto>> GetUsersLike(LikeParams likeParams);
    }
}