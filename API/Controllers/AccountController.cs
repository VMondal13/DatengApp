using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    public AccountController(DataContext context, ITokenService tokenService)
    {
        this._context = context;
        this._tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> RegisterUser(RegisterUserDTO registerUserDTO)
    {
        if(await UserExists(registerUserDTO.username))
            return BadRequest("user is already taken");
        using var hmac = new HMACSHA512();
        AppUser user = new AppUser();
        user.UserName = registerUserDTO.username.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerUserDTO.password));
        user.PasswordSalt = hmac.Key; //randomly generated key

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new UserDTO
        {
            UserName = user.UserName,
            token = _tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> LoginrUser(LoginDTO loginDTO)
    {
        var user = await _context.Users.Include( x => x.Photos).SingleOrDefaultAsync(x => x.UserName == loginDTO.username);
        if(user == null)
        {
            return Unauthorized("Invalid UserName");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));
        for(int i = 0; i < computedHash.Length; i++)
        {
            if(computedHash[i] != user.PasswordHash[i])
                return Unauthorized("Invalid Password");
        }
        UserDTO userDTO = new UserDTO();
        userDTO.UserName = user.UserName;
        userDTO.token = _tokenService.CreateToken(user);
        userDTO.photoUrl = user.Photos.FirstOrDefault( x => x.IsMain)?.Url;
        
        return userDTO;        
    }

    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
