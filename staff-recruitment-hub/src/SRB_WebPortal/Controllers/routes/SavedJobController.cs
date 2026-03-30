using Microsoft.AspNetCore.Mvc;

using SRB_WebPortal.Data;

namespace SRB_WebPortal.Controllers.routes;

public class SavedJobController : Controller
{
   public IActionResult Index()
   {
      var savedJobs = JobMock.GetMockJobPosts();

      return View(savedJobs);
   }
}
