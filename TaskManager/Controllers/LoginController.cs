using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;

public class LoginController : Controller
{
    private readonly IUsuarioRepository usuarioRep;
    private readonly ILogger<LoginController> _logger;

    public LoginController(IUsuarioRepository usuarioRepository, ILogger<LoginController> logger)
    {
        usuarioRep = usuarioRepository;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(new LoginVM());
    }
}