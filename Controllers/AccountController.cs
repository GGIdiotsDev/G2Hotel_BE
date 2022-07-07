using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.DTOs;
using g2hotel_server.Entities;
using g2hotel_server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator;

namespace g2hotel_server.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService _mailService;
        public AccountController(ITokenService tokenService, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMailService mailService)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mailService = mailService;
        }

        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<UserUpdateDTO>> GetUser(string username)
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (user == null) return NotFound();

            var currentUser = _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            if (currentUser != user.UserName) return NotFound();

            var userDto = _mapper.Map<UserUpdateDTO>(user);

            return Ok(userDto);
        }

        [HttpPut("update-info")]
        [Authorize]
        public async Task<ActionResult> UpdateInfoUser(UserUpdateDTO memberUpdateDto)
        {

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(memberUpdateDto.Username);

            _mapper.Map(memberUpdateDto, user);

            _unitOfWork.UserRepository.Update(user);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<ActionResult> UpdatePassword(MemberUpdateDTO memberUpdateDto)
        {

            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(memberUpdateDto.Username);

            var result = await _userManager.ChangePasswordAsync(user, memberUpdateDto.CurrentPassword, memberUpdateDto.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            _unitOfWork.UserRepository.Update(user);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("reset-password")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ForgotMailDTO mailDTO)
        {

            var user = await _userManager.FindByEmailAsync(mailDTO.Email);

            if (user == null)
            {
                return BadRequest("Email không tồn tại");
            }
            await _userManager.RemovePasswordAsync(user);

            var pwd = new Password().IncludeLowercase().IncludeUppercase().IncludeSpecial().IncludeNumeric().LengthRequired(16);
            var password = pwd.Next();

            await _userManager.AddPasswordAsync(user, password);
            // Gửi email
            MailDTO mailReceived = new MailDTO
            {
                To = mailDTO.Email,
                Subject = "Reset Password",
                Html = $"Bạn đã yêu cầu đổi mật khẩu! Đây là mật khẩu mới của bạn <span style='color:blue;font-size:14px;font-weight:'700'>{password}</span>"
            };
            _mailService.Send(mailReceived);

            // Chuyển đến trang thông báo đã gửi mail để reset password
            return Ok();
        }
    }
}