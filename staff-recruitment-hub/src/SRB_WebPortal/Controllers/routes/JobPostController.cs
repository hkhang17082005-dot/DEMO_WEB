using Microsoft.AspNetCore.Mvc;

using SRB_WebPortal.Shared;

using SRB_WebPortal.Controllers.apis.post;

namespace SRB_WebPortal.Controllers.routes;

[Route("job-post")]
public class JobPostController(IShareRepository shareRepository) : Controller
{
   private readonly IShareRepository _shareRepository = shareRepository;

   [HttpGet("JobPostID={jobPostId}")]
   public async Task<IActionResult> Index(string jobPostId)
   {
      var foundJobPost = await _shareRepository.GetJobPost(jobPostId);

      if (foundJobPost is null)
      {
         return NotFound();
      }

      var resPost = new JobPostDTO
      {
         JobPostID = jobPostId,
         Title = foundJobPost.Title,
         BusinessID = foundJobPost.BusinessID,
         BusinessName = foundJobPost.Business.BusinessName,
         BusinessLogoURL = foundJobPost.Business.LogoUrl,
         JobType = foundJobPost.JobType,
         SalaryRange = foundJobPost.SalaryRange,
         Description = foundJobPost.Description,
         Requirements = foundJobPost.Requirements,
         Benefits = foundJobPost.Benefits,
         LocationID = foundJobPost.LocationID,
         Address = foundJobPost.Address,
         ExpiryDate = foundJobPost.ExpiryDate,
         CreatedAt = foundJobPost.CreatedAt
      };

      ViewBag.Locations = await _shareRepository.GetLocations();

      return View(resPost);
   }
}

