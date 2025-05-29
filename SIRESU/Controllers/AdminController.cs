using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    public IActionResult Panel()
    {
        return View();
    }
}
