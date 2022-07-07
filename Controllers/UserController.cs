using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.DTOs;
using g2hotel_server.Entities;
using g2hotel_server.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace g2hotel_server.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(ITokenService tokenService, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IUnitOfWork unitOfWork)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username)) return BadRequest("Username already exists");
            var user = _mapper.Map<AppUser>(registerDTO);
            user.UserName = registerDTO.Username.ToLower();
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded) return BadRequest(result.Errors);

            // var roleResult = await _userManager.AddToRoleAsync(user, "Member");
            var roleResult = await _userManager.AddToRolesAsync(user, registerDTO.Roles);
            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDTO
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Invalid username");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            return new UserDTO
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url
            };
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetAllUsers()
        {
            var users = await _unitOfWork.UserRepository.GetUsersAsync(); ;
            var usersDto = _mapper.Map<IEnumerable<MemberDTO>>(users);
            return Ok(usersDto);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("{username}")]
        public async Task<ActionResult<UserUpdateDTO>> GetUser(string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();

            //get all roles of user
            var roles = await _userManager.GetRolesAsync(user);

            var userDto = _mapper.Map<UserUpdateDTO>(user);
            userDto.Roles = roles;

            return Ok(userDto);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut]
        public async Task<ActionResult> UpdateUser(UserUpdateDTO memberUpdateDto)
        {

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(memberUpdateDto.Username);

            _mapper.Map(memberUpdateDto, user);
            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            await _userManager.AddToRolesAsync(user, memberUpdateDto.Roles);

            _unitOfWork.UserRepository.Update(user);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update user");
        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("del/{username}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound("User does not exist");
            _unitOfWork.UserRepository.Delete(user);
            if (await _unitOfWork.Complete()) return Ok();
            return BadRequest("Failed to delete user");
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}