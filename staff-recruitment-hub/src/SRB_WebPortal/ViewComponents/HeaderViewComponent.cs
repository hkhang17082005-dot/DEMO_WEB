using Microsoft.AspNetCore.Mvc;

namespace SRB_WebPortal.ViewComponents
{
   public class HeaderViewComponent : ViewComponent
   {
      public async Task<IViewComponentResult> InvokeAsync()
      {
         return View();
      }
   }
}
