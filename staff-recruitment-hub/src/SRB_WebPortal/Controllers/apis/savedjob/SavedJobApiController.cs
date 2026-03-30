using Microsoft.AspNetCore.Mvc;
using SRB_ViewModel;
using SRB_ViewModel.Entities;
using System.Security.Claims;

namespace SRB_WebPortal.Controllers.apis.savedjob;

[ApiController]
[Route("api/[controller]")]
public class SavedJobController(DatabaseContext context) : ControllerBase
{
   private readonly DatabaseContext _context = context;

   [HttpPost("toggle")]
   public IActionResult Toggle([FromBody] dynamic data)
   {
      int jobId = (int)data.jobId;

      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      if (userId == null)
         return Unauthorized();

      var existing = _context.SavedJobs
          .FirstOrDefault(x => x.JobID == jobId && x.UserId == userId);

      if (existing != null)
      {
         _context.SavedJobs.Remove(existing);
      }
      else
      {
         _context.SavedJobs.Add(new SavedJob
         {
            JobID = jobId,
            UserId = userId
         });
      }

      _context.SaveChanges();

      return Ok();
   }
}
