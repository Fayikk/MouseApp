using API.Dtos;
using API.Entity;
using API.Extensions;
using API.Helper;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        private readonly IImageService _imageService;
        private readonly IUnitOfWork _uow;
        
        public UserController(IUserRepository userRepository 
                             ,IMapper mapper
                             ,IUnitOfWork uow
                             ,IImageService imageService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _uow = uow;
            _imageService = imageService;
        }
       [HttpGet]
        public async Task <ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery]UserParams userParams){
           
           var position  = await _uow.UserRepository.GetUserPosition(User.GetUserName());
           userParams.CurrentUsername = User.GetUserName();

            if (string.IsNullOrEmpty(userParams.Position))
            {
                userParams.Position = position == "employee" ? "employer" : "employee";   
            }

           
            var users = await _uow.UserRepository.GetMemberAsync(userParams);
            
              Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, 
                users.TotalCount, users.TotalPages));

            return Ok(users);
        }

         [Authorize]
        [HttpPost("photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto([FromForm] IFormFile file){
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUserName());
            if (user == null)
            {
                return NotFound();
            }
            var result = await _imageService.AddPhotoAsync(file);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            var photo = new Photo{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.Photos.Count == 0){
                photo.IsMain = true;
            }
            user.Photos.Add(photo);

            if (await _uow.Complete()) 
            {
                return CreatedAtAction(nameof(GetUser),
                new {sername = user.UserName},_mapper.Map<PhotoDto>(photo));
            }
            return BadRequest("Problem adding photo");
        }


        [HttpPut("update")]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUserName());
            if (user == null)
            {
                return NotFound();
            }
            _mapper.Map(memberUpdateDto,user);
            if (await _uow.Complete())
            {
                return NoContent();
            }
            return BadRequest("Failed to update user");
        }



         [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId){
            //For change us account profile image
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUserName());
            if(user == null) return NotFound();
            var photo = user.Photos.FirstOrDefault(x=>x.Id == photoId);
            if(photo == null) return NotFound();
            if (photo.IsMain)
            {
                return BadRequest("this is already your main photo");
            }
            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain !=null)
            {
                currentMain.IsMain = false;
            }
            photo.IsMain = true;

            if (await _uow.Complete())
            {
                return NoContent();
            }
            return BadRequest("Problem seting the main photo");
        } 


        // [AllowAnonymous]
         [HttpGet("{username}")]
        public  async Task<ActionResult<MemberDto>> GetUser(string username){
            
            return await _uow.UserRepository.GetMemberAsync(username);
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId){
            var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUserName());
            var photo = user.Photos.FirstOrDefault(a => a.Id == photoId);
            if (photo == null)
            {
                return NotFound();
            }
            if (photo.IsMain)
            {
                return BadRequest("You cannot delete your main photo");
            }
            if (photo.PublicId != null)
            {
                var result = await _imageService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null)
                {
                    return BadRequest(result.Error.Message);
                }
            }
            user.Photos.Remove(photo);
            if (await _uow.Complete())
            {
                return Ok();
            }
            return BadRequest("Please refresh the page");
        }
    }
}