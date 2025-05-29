using Microsoft.AspNetCore.Mvc;

public class ClienteController : Controller
{
    public IActionResult Inicio()
    {
        return View();
    }
}
