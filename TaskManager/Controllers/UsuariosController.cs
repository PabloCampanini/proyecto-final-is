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
        try
        {
            if (!ValidarRol()) return RedirectToAction("Index", "Home");

            ListarUsuariosVM usuariosVM = new ListarUsuariosVM(usuarioRep.GetAllUsuarios());

            return View(usuariosVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener los usuarios");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult CrearUsuario()
    {
        try
        {
            if(!ValidarRol()) return RedirectToAction("Index", "Home");

            return View(new CrearUsuarioVM());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar un usuario nuevo");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult CrearUsuario(CrearUsuarioVM usuarioCargado)
    {
        try
        {
            if(!ValidarRol()) return RedirectToAction("Index", "Home");
            usuarioRep.CreateUsuario(usuarioCargado.UsuarioNuevo);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar un usuario nuevo");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult ModificarUsuario(int idUsuarioB)
    {
        try
        {
            if(!ValidarRol()) return RedirectToAction("Index", "Home");
            ModificarUsuarioVM usuarioM = new ModificarUsuarioVM();
            usuarioM.UsuarioAModificar =
                usuarioRep.GetUsuarioById(idUsuarioB);

            return View(usuarioM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al modificar un usuario");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult ModificarUsuario(ModificarUsuarioVM usuarioM)
    {
        try
        {
            if(!ValidarRol()) return RedirectToAction("Index", "Home");
            usuarioRep.UpdateUsuario(
                usuarioM.UsuarioAModificar.IdUsuario,
                usuarioM.UsuarioAModificar
            );

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al modificar un usuario");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult Password(int idUsuario)
    {
        try
        {
            var IdPropietario = ValidarSesion();
        
            if (!IdPropietario.HasValue || IdPropietario.Value != idUsuario) return RedirectToAction("Index", "Home"); //Hago que solo el propio usuario pueda cambiar su contraseña
        
            ModificarContraseñaVM contraseñaVM = new ModificarContraseñaVM();

            contraseñaVM.IdUsuarioB = idUsuario;
            contraseñaVM.ActualPassword = usuarioRep.GetUsuarioById(idUsuario).Password;

            return View(contraseñaVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar la contraseña");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult Password(ModificarContraseñaVM contraseñaVM)
    {
        try
        {
            var IdPropietario = ValidarSesion();
        
            if (!IdPropietario.HasValue || IdPropietario.Value != contraseñaVM.IdUsuarioB) return RedirectToAction("Index", "Home");
        
            usuarioRep.ChangePassword(
                contraseñaVM.IdUsuarioB,
                contraseñaVM.newPassword
            );

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar la contraseña");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult BorrarUsuario(int idUsuarioB)
    {
        try
        {
            if(!ValidarRol()) return RedirectToAction("Index", "Home");

            var cantidadTableros = tableroRep.GetAllTablerosByIdCreador(idUsuarioB).Count;
        
            if (cantidadTableros != 0)
            {
                return RedirectToAction("IncapazBorrarUsuario", new{idUsuarioB = idUsuarioB});
            }

            return View(usuarioRep.GetUsuarioById(idUsuarioB));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al borrar un usuario");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult BorrarUsuario(Usuarios usuarioB)
    {
        try
        {
            if(!ValidarRol()) return RedirectToAction("Index", "Home");
            usuarioRep.DeleteUsuario(usuarioB.IdUsuario);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al borrar un usuario");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult IncapazBorrarUsuario(int idUsuarioB)
    {
        try
        {
            if(!ValidarRol()) return RedirectToAction("Index", "Home");
    
            return View(idUsuarioB);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al borrar un usuario");
            return RedirectToAction("Error");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}