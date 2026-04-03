using Microsoft.AspNetCore.Mvc;

using SRB_ViewModel;
using SRB_ViewModel.Entities;

namespace SRB_WebPortal.Controllers.apis.user;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService service, DatabaseContext context) : ControllerBase
{
   private readonly DatabaseContext _context = context;
   private readonly IUserService _service = service;

   [HttpGet]
   public async Task<IActionResult> GetUsers() =>
        Ok(await _service.GetAllUsersAsync());

   [HttpPost]
   public async Task<IActionResult> CreateUser(UserCreateDto dto) =>
       Ok(await _service.CreateUserAsync(dto));

   [HttpPut("{id}")]
   public async Task<IActionResult> UpdateUser(string id, UserUpdateDto dto) =>
       Ok(await _service.UpdateUserAsync(id, dto));


   [HttpPost("{id}/lock")]
   public async Task<IActionResult> ToggleLockUser(string id)
   {
      var user = await _context.Users.FindAsync(id);
      if (user == null) return NotFound(new { success = false });

      // Logic đảo trạng thái
      if (user.Status == UserStatus.locked)
      {
         user.Status = UserStatus.inactive;
      }
      else
      {
         user.Status = UserStatus.locked;
      }

      user.UpdatedAt = DateTime.Now;
      await _context.SaveChangesAsync();

      return Ok(new
      {
         success = true,
         currentStatus = user.Status.ToString()
      });
   }
   [HttpDelete("{id}")]
   public async Task<IActionResult> DeleteUser(string id) =>
       Ok(await _service.DeleteUserAsync(id));
}
