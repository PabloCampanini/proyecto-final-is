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

    [HttpPost]
    public IActionResult Login(LoginVM UsuarioCargado)
    {
        var usuario = usuarioRep.GetAllUsuarios().FirstOrDefault(u => u.Email == UsuarioCargado.Email && u.Password == UsuarioCargado.Password);

        if (usuario == null)
        {
            string accesoRechazado = "Intento de acceso invalido - Usuario: " + UsuarioCargado.Email + " - Clave: " + UsuarioCargado.Password;

            Console.WriteLine(accesoRechazado);
            _logger.LogWarning(accesoRechazado);

            UsuarioCargado.ErrorMessage = "Usuario o Contraseña incorrectos. Ingrese sus datos nuevamente";

            return View("Index", UsuarioCargado);
        }

        string accesoExitoso = "El usuario " + usuario.Email + " ingreso correctamente";

        Console.WriteLine(accesoExitoso);
        _logger.LogInformation(accesoExitoso);

        HttpContext.Session.SetString("rolSesion", usuario.Rol.ToString());
        HttpContext.Session.SetString("usuario", usuario.NombreDeUsuario);
        HttpContext.Session.SetInt32("idSesion", usuario.IdUsuario);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        string mensajeLogout = "Sesión cerrada correctamente.";

        Console.WriteLine(mensajeLogout);
        _logger.LogInformation(mensajeLogout);

        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}