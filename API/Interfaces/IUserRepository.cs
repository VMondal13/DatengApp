using API.DTOs;
using API.Entities;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<AppUser> GetUserByUsernameAsync(string username);
    Task<AppUser> GetUserByIdAsync(int id);
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<bool> SaveAllAsync();
    void Update(AppUser user);
    Task<IEnumerable<MemberDTO>> GetMembersAsync();
    Task<MemberDTO> GetMemberByUsernameAsync(string username);

}
