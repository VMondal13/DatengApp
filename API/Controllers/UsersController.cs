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
    private readonly IPhotoService _photoService;
    public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
    {
        //this._context = context;
        _mapper = mapper;
        _userRepository = userRepository;
        _photoService = photoService;

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

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;  

        var user = await _userRepository.GetUserByUsernameAsync(username);

        if(user == null) return NotFound();

        var result = await _photoService.AddPhotoAsync(file);

        if(result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url =  result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if(user.Photos.Count == 0)
            photo.IsMain = true;
        
        user.Photos.Add(photo);

        if(await _userRepository.SaveAllAsync())
        {
            //return _mapper.Map<PhotoDTO>(photo);
            return CreatedAtAction(nameof(GetUser), new {username = user.UserName}, _mapper.Map<PhotoDTO>(photo));

        }
        return BadRequest("Problem adding photo");

    }

    [HttpPut("set-main-photo/{photoId}")] 
    public async Task<ActionResult> UpdateMainPhoto(int photoId)
    {
       var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;    

       var user = await _userRepository.GetUserByUsernameAsync(username);

       if(user == null) return NotFound();

       var photo = user.Photos.FirstOrDefault( x => x.Id == photoId);

       if(photo !=null)
        {
            if(photo.IsMain) return BadRequest("Photo is already set as main");
            var currentMain = user.Photos.FirstOrDefault( x => x.IsMain);
            if(currentMain != null)
                currentMain.IsMain = false;
            photo.IsMain = true;

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Problem setting the main photo");

        }

       else return NotFound();

    }

    [HttpDelete("delete-photo/{photoId}")] 
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
       var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
       var user = await _userRepository.GetUserByUsernameAsync(username);
       if(user == null) return NotFound();

       var photo = user.Photos.FirstOrDefault( x => x.Id == photoId);
       if(photo !=null)
       {
            if(photo.IsMain) return BadRequest("Main photo can not be deleted.");

            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);                
            }
            user.Photos.Remove(photo);

            if(await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Problem deleting the photo");
       }
       else return NotFound();

    }

}
