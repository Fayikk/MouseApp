using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Entity;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controller
{
    public class AccountController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        
        private readonly UserManager<BusinessUser> _userManager;
        public AccountController(IMapper mapper, ITokenService tokenService,UserManager<BusinessUser> userManager)  
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost("register")] //api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){

       
            if(await UserExist(registerDto.Username)) return BadRequest("Username is taken");
    
           var user = _mapper.Map<BusinessUser>(registerDto);
    
         
          
            user.UserName = registerDto.Username.ToLower();
           
            var result = await _userManager.CreateAsync(user , registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
           
            // var roleResult = await _userManager.AddToRoleAsync(user,"Member");
            // if (!roleResult.Succeeded)
            // {
            //     return BadRequest(result.Errors);
            // }

           return new UserDto{
            Username = user.UserName,
            Token =await _tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
            Gender = user.Gender,
            Position = user.Position
           };
        }
         public async Task<bool> UserExist(string username){

            return await _userManager.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }
         [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
            var user = await _userManager.Users
            .Include(x=> x.Photos) //For navbar image
            .SingleOrDefaultAsync(x =>  x.UserName == loginDto.Username);
   
            if(user == null)    return Unauthorized("Invalid Username");

            var result = await _userManager.CheckPasswordAsync(user , loginDto.Password);

            if (!result)
            {
                return Unauthorized("Invalid password");
            }


            return new UserDto{
                Username = user.UserName,
                Token =await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                Gender = user.Gender,
                Position=user.Position
            };
       
        }
    }
}