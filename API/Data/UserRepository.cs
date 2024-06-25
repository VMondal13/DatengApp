using System.Collections.Immutable;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public UserRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _context.Users
        .Include(x => x.Photos)
        .ToListAsync();
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await _context.Users
        .Include(x => x.Photos)
        .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified; //doesn't necessarily saves the updated data
    }

    public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
    {
        return await _context.Users        
        .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<MemberDTO> GetMemberByUsernameAsync(string username)
    {
        return await _context.Users
        .Where(x => x.UserName == username)
        .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
        .SingleOrDefaultAsync();
        
    }
}
