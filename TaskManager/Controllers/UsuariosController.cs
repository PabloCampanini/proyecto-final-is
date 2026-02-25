using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
public class UsuariosController : ValidacionesController
{
    private readonly IUsuarioRepository usuarioRep;
    private readonly ITableroRepository tableroRep;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(IUsuarioRepository usuarioRepository, ITableroRepository tableroRepository, 
    ILogger<UsuariosController> logger) : base(tableroRepository)
    {
        usuarioRep = usuarioRepository;
        tableroRep = tableroRepository;
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (!ValidarRol()) return RedirectToAction("Index", "Home");

        ListarUsuariosVM usuariosVM = new ListarUsuariosVM(usuarioRep.GetAllUsuarios());

        return View(usuariosVM);
    }

    [HttpGet]
    public IActionResult CrearUsuario()
    {
        if(!ValidarRol()) return RedirectToAction("Index", "Home");

        return View(new CrearUsuarioVM());
    }

    [HttpPost]
    public IActionResult CrearUsuario(CrearUsuarioVM usuarioCargado)
    {
        if(!ValidarRol()) return RedirectToAction("Index", "Home");
        usuarioRep.CreateUsuario(usuarioCargado.UsuarioNuevo);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModificarUsuario(int idUsuarioB)
    {
        if(!ValidarRol()) return RedirectToAction("Index", "Home");
        ModificarUsuarioVM usuarioM = new ModificarUsuarioVM();
        usuarioM.UsuarioAModificar =
            usuarioRep.GetUsuarioById(idUsuarioB);

        return View(usuarioM);
    }

    [HttpPost]
    public IActionResult ModificarUsuario(ModificarUsuarioVM usuarioM)
    {
        if(!ValidarRol()) return RedirectToAction("Index", "Home");
        usuarioRep.UpdateUsuario(
            usuarioM.UsuarioAModificar.IdUsuario,
            usuarioM.UsuarioAModificar
        );

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Password(int idUsuario)
    {
        var IdPropietario = ValidarSesion();
    
        if (!IdPropietario.HasValue || IdPropietario.Value != idUsuario) return RedirectToAction("Index", "Home"); //Hago que solo el propio usuario pueda cambiar su contraseña
    
        ModificarContraseñaVM contraseñaVM = new ModificarContraseñaVM();

        contraseñaVM.IdUsuarioB = idUsuario;
        contraseñaVM.ActualPassword = usuarioRep.GetUsuarioById(idUsuario).Password;

        return View(contraseñaVM);
    }

    [HttpPost]
    public IActionResult Password(ModificarContraseñaVM contraseñaVM)
    {
        var IdPropietario = ValidarSesion();
    
        if (!IdPropietario.HasValue || IdPropietario.Value != contraseñaVM.IdUsuarioB) return RedirectToAction("Index", "Home");
    
        usuarioRep.ChangePassword(
            contraseñaVM.IdUsuarioB,
            contraseñaVM.newPassword
        );

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult BorrarUsuario(int idUsuarioB)
    {
        if(!ValidarRol()) return RedirectToAction("Index", "Home");

        var cantidadTableros = tableroRep.GetAllTablerosByIdCreador(idUsuarioB).Count;
    
        if (cantidadTableros != 0)
        {
            return RedirectToAction("IncapazBorrarUsuario", new{idUsuarioB = idUsuarioB});
        }

        return View(usuarioRep.GetUsuarioById(idUsuarioB));
    }

    [HttpPost]
    public IActionResult BorrarUsuario(Usuarios usuarioB)
    {
        if(!ValidarRol()) return RedirectToAction("Index", "Home");
        usuarioRep.DeleteUsuario(usuarioB.IdUsuario);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult IncapazBorrarUsuario(int idUsuarioB)
    {
        if(!ValidarRol()) return RedirectToAction("Index", "Home");
        return View(idUsuarioB);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}