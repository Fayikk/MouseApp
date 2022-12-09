using API.Dtos;
using API.Entity;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(BusinessUser user);
        Task<IEnumerable<BusinessUser>> GetUsersAsync();
        Task<BusinessUser> GetUserByIdAsync(int id);
        Task<BusinessUser> GetUserByUsernameAsync(string username);
        Task<MemberDto> GetMemberAsync(string username);
        Task<string> GetUserPosition(string username); 
        Task<PagedList<MemberDto>> GetMemberAsync(UserParams userParams);
           
       
    }
}   