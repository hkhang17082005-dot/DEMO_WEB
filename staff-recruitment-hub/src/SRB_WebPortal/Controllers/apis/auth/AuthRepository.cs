using Microsoft.EntityFrameworkCore;

using SRB_ViewModel;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.auth;

public interface IAuthRepository
{
   Task<int> GetRoleIDBySlug(string roleSlug);

   Task<string?> GetRoleSlugByID(int roleID);

   /// <summary>
   /// Kiểm tra xem tên đăng nhập đã tồn tại trong hệ thống hay chưa
   /// </summary>
   /// <returns><b>True</b> nếu đã tồn tại hoặc có lỗi; <b>False</b> nếu có thể sử dụng</returns>
   Task<bool> ExistingUsername(string username);

   Task CreateNewUser(string userID, string username, string hashedPassword, int roleID);

   Task<User?> GetUserByUserID(string userID);

   Task<User?> GetUserByUsername(string username);

   Task SaveProfile(CreateProfileDTO profileDTO, string userID, string? avatarURL, string? cvURL);

   Task<UserMeResponse?> GetMe(string userID);

   Task<string[]> GetUserRoleSlugs(string userID);
}

public class AuthRepository(DatabaseContext context) : IAuthRepository
{
   private readonly DatabaseContext _context = context;

   public async Task<string[]> GetUserRoleSlugs(string userID)
   {
      try
      {
         return await _context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserID == userID)
            .Select(ur => ur.Role.RoleSlug)
            .ToArrayAsync();
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in GetUserRoleSlugs: {ex.Message}");

         return [];
      }
   }

   public async Task<UserMeResponse?> GetMe(string userID)
   {
      try
      {
         var userRoleSlugs = await GetUserRoleSlugs(userID);

         if (userRoleSlugs is null || userRoleSlugs.Length == 0)
         {
            return null;
         }

         return await _context.Users
            .AsNoTracking()
            .Include(u => u.UserProfile)
            .Where(u => u.UserID == userID)
            .Select(u => new UserMeResponse
            {
               UserID = u.UserID,
               Username = u.Username,
               RoleSlugs = userRoleSlugs,
               BusinessID = u.BusinessID,
               // Lấy UserProfile
               FullName = u.UserProfile != null ? u.UserProfile.FullName : string.Empty,
               Email = u.UserProfile != null ? u.UserProfile.Email : string.Empty,
               PhoneNumber = u.UserProfile != null ? u.UserProfile.PhoneNumber : null,
               AvatarURL = u.UserProfile != null ? u.UserProfile.AvatarURL : null,
               CVvURL = u.UserProfile != null ? u.UserProfile.CVPath : null
            })
            .FirstOrDefaultAsync();
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in GetMe Repository: {ex.Message}");

         return null;
      }
   }

   public async Task<int> GetRoleIDBySlug(string roleSlug)
   {
      try
      {
         if (string.IsNullOrWhiteSpace(roleSlug)) return -1;

         return await _context.Roles
            .AsNoTracking()
            .Where(r => r.RoleSlug == roleSlug)
            .Select(r => r.RoleID)
            .FirstOrDefaultAsync();
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in GetRoleIDByName: {ex.Message}");
         return -1;
      }
   }

   public async Task<string?> GetRoleSlugByID(int roleID)
   {
      try
      {
         return await _context.Roles
            .AsNoTracking() // Chỉ đọc không Cập nhật Database
            .Where(r => r.RoleID == roleID)
            .Select(r => r.RoleSlug)
            .FirstOrDefaultAsync();
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in GetRoleNameByID: {ex.Message}");
         return null;
      }
   }

   public async Task<bool> ExistingUsername(string username)
   {
      try
      {
         if (string.IsNullOrWhiteSpace(username)) return true;

         return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Username == username);
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in ExistingUsername: {ex.Message}");
         return true;
      }
   }

   public async Task CreateNewUser(string userID, string username, string hashedPassword, int roleDefaultID)
   {
      try
      {
         var newUser = new User
         {
            UserID = userID,
            Username = username,
            HashPassword = hashedPassword,
            UserRoles =
            [
               new UserRoles
               {
                  UserID = userID,
                  RoleID = roleDefaultID
               }
            ]
         };

         await _context.Users.AddAsync(newUser);
         await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in Create New User: {ex.Message}");
      }
   }

   public async Task<User?> GetUserByUserID(string userID)
   {
      if (string.IsNullOrWhiteSpace(userID)) return null;
      try
      {
         var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UserID == userID);

         return user;
      }
      catch (Exception ex)
      {
         Console.WriteLine($"Error in GetUserByUsername: {ex.Message}");
         throw;
      }
   }

   public async Task<User?> GetUserByUsername(string username)
   {
      if (string.IsNullOrWhiteSpace(username)) return null;
      try
      {
         var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == username);

         return user;
      }
      catch (Exception ex)
      {
         Console.WriteLine($"Error in GetUserByUsername: {ex.Message}");
         throw;
      }
   }

   public async Task SaveProfile(CreateProfileDTO profileDTO, string userID, string? avatarURL, string? cvURL)
   {
      try
      {
         var newUserProfile = new UserProfile
         {
            UserID = userID,
            FullName = profileDTO.FullName,
            Email = profileDTO.Email,
            PhoneNumber = profileDTO.PhoneNumber,
            Summary = profileDTO.Summary,
            CVPath = cvURL,
            AvatarURL = avatarURL,
            Gender = profileDTO.Gender,
            Birthday = profileDTO.Birthday,
            Address = profileDTO.Address
         };

         await _context.UserProfiles.AddAsync(newUserProfile);
         await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
         Console.Error.WriteLine($"Error in Create New UserProfile: {ex.Message}");
      }
   }
}
