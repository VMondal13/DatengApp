using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
//api/users
public class UsersController : BaseApiController
{
    //private readonly DataContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        //this._context = context;
        _mapper = mapper;
        _userRepository = userRepository;

    }

    //[AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        // var users = await _context.Users.ToListAsync();
        // return users;

        // var users = await _userRepository.GetUsersAsync();
        // var returnToUsers = _mapper.Map<IEnumerable<MemberDTO>>(users);
        // return Ok(returnToUsers);

       return Ok(await _userRepository.GetMembersAsync());

    }

    [HttpGet("{username}")] //api/users/2
    public async Task<ActionResult<MemberDTO>> GetUser(string username)
    {
        return await _userRepository.GetMemberByUsernameAsync(username);      
        
        
        // var users = await _userRepository.GetUserByUsernameAsync(username);
        // var returnToUsers = _mapper.Map<MemberDTO>(users); 
        // return returnToUsers;
        
        //return await _context.Users.FindAsync(id);
        //return user;

    }

    [HttpPut] 
    public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
    {
       var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;    

       var user = await _userRepository.GetUserByUsernameAsync(username);

       if(user == null) return NotFound();

       _mapper.Map(memberUpdateDTO, user);

        if(await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("failed to save changes");

    }
}
