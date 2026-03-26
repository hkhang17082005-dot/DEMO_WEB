using Microsoft.AspNetCore.Mvc;

namespace SRB_WebPortal.Controllers.routes;

public class Create_CVController : Controller
{
   public IActionResult Index()
   {
      return View();
   }

   // CV đơn giản
   public IActionResult Simple_CV()
   {
      return View("Simple_CV/Index");
   }

   // CV chuyên nghiệp
   public IActionResult CVChuyenNghiep()
   {
      return View("CVChuyenNghiep/Index");
   }
   // CV hiện đại
   public IActionResult Modern_CV()
   {
      return View("Modern_CV/Index");
   }
   //Tất cả mẫu CV theo Style
   public IActionResult All_CV_Style()
   {
      return View("All_CV_Style/Index");
   }
   public IActionResult CV_Management()
   {
      return View("CV_Management/Index");
   }
   public IActionResult Upload_CV()
   {
      return View("Upload_CV/Index");
   }
   public IActionResult Create()
   {
      return View("Create/Index");
   }

}
