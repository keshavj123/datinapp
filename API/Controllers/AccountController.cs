#nullable enable
using Microsoft.AspNetCore.Mvc;
using API.Data;
using System.Threading.Tasks;
using API.Entities;
using API.Data.Migrations;
using System.Security.Cryptography;
using System.Text;
using System;
using API.DTOs;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using API.Services;


namespace API.Controllers

{
    public class AccountController: BaseAPIController
    {
        private readonly  DataContext _context;
        private readonly ITokenService _tokenservice;

         public AccountController (DataContext context, ITokenService Tokenservice)
         {
             _tokenservice=Tokenservice;
             _context=context;
         }

        [HttpPost("register")]

        public async Task<ActionResult<UserDTO>> Register(RegisterDto registerDto)
        {

            if(await UserExists(registerDto.Username)) return BadRequest("Username is taken");


            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
            
               UserName=registerDto.Username.ToLower(),
                PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt=hmac.Key

            };
            

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

            return new UserDTO
            {
                username =user.UserName,
                Token= _tokenservice.CreateToken(user)
            };
        }

        [HttpPost("login")]

        public async Task<ActionResult<UserDTO>> Login(LoginDTO logindto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName ==logindto.Username);

            if (user==null) return Unauthorized("Invalid username");

            using var hmac= new HMACSHA512(user.PasswordSalt);

            var computedHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(logindto.Password));

            for (int i=0;i<computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return new UserDTO
            {
                username= user.UserName,
                Token = _tokenservice.CreateToken(user)
            };
        }

        private async Task<bool>UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName==username.ToLower());
        }
    }
}