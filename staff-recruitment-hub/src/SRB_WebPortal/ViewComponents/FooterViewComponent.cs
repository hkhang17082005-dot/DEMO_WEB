using Microsoft.AspNetCore.Mvc;

namespace SRB_WebPortal.ViewComponents
{
   public class FooterViewComponent : ViewComponent
   {
      public async Task<IViewComponentResult> InvokeAsync()
      {
         return View();
      }
   }
}
