using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RideRate.DTOModels;
using RideRate.Helpers;
using RideRate.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RideRate.Data;
using Microsoft.EntityFrameworkCore;

namespace RideRate.Services.Auth
{
    public class LoginServices : ILoginServices
    {
        private IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _iHttpContextAccessor;
        private readonly ApiDbcontext _context;
        public LoginServices
        (
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor iHttpContextAccessor,
            ApiDbcontext context   
        )
        {
            _configuration= configuration;
            _userManager= userManager;
            _iHttpContextAccessor= iHttpContextAccessor;
            _context= context;

        }
        public async Task<GenericResponse<string>> Login(LoginDTO request)
        {
            try
            {
                var userExist = await _userManager.FindByEmailAsync(request.EmailAddress);
                var passwordCheck = await _userManager.CheckPasswordAsync(userExist, request.Password);
                if (userExist != null && passwordCheck)
                {
                    var token = CreateToken(userExist).Result;
                    var refreshToken = GenerateRefreshToken();
                    SetRefreshToken(refreshToken, userExist);
                    return new GenericResponse<string>
                    {
                        StatusCode = HttpStatusCode.OK,
                        Data = token,
                        Message = "Login successful",
                        Success = true
                    };
                }
                return new GenericResponse<string>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Data = null,
                    Message = "Incorrect Password Or User Dosen't Exist",
                    Success = false
                };
            }
            catch(Exception ex)
            {
                return new GenericResponse<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"An error occurred: {ex.Message}",
                    Success = false
                };
            }
        }

        public async Task<GenericResponse<string>> RefreshToken()
        {
            try
            {
                var refreshToken = _iHttpContextAccessor.HttpContext.Request.Cookies["refreshToken"];

                var tokenUser = await _context.ApplicationUser.FirstOrDefaultAsync(u=> u.RefershToken == refreshToken);

                if (tokenUser is null)
                {
                    return new GenericResponse<string>
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Data = null,
                        Message = "Invalid Refresh Token",
                        Success = false
                    };
                }
                else if(tokenUser.TokenExpires < DateTime.Now)
                {
                    return new GenericResponse<string>
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Data = null,
                        Message = "Token Expired",
                        Success = false
                    };
                }

                string token = CreateToken(tokenUser).Result;
                var newRefreshToken = GenerateRefreshToken();
                SetRefreshToken(newRefreshToken, tokenUser);

                return new GenericResponse<string>
                {
                    StatusCode = HttpStatusCode.OK,
                    Data = token,
                    Message = "Token Refreshed Successfully",
                    Success = true

                };
            } 
            catch(Exception ex) 
            {
                return new GenericResponse<string>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Message = $"An error occurred: {ex.Message}",
                    Success = false
                };
            }

        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created= DateTime.Now
            };
            return refreshToken; 
        }
        private async void SetRefreshToken(RefreshToken newRefreshToken, ApplicationUser user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            _iHttpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefershToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            _context.Update(user);
            await _context.SaveChangesAsync();
            
        }

        private async Task<string> CreateToken(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JwtTokens:Key").Value));
            var creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials:creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
