using Microsoft.AspNetCore.Mvc;

public class Create_CVController : Controller
{
    // Trang chính (nếu có)
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
}