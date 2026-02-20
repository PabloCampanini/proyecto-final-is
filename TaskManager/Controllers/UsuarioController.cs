using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
public class UsuariosController : Controller
{
    private readonly IUsuarioRepository usuarioRep;

    public UsuariosController(IUsuarioRepository usuarioRepository)
    {
        usuarioRep = usuarioRepository;
    }

    public IActionResult Index()
    {
        ListarUsuariosVM usuariosVM =
            new ListarUsuariosVM(usuarioRep.GetAllUsuarios());

        return View(usuariosVM);
    }

    [HttpGet]
    public IActionResult CrearUsuario()
    {
        return View(new CrearUsuarioVM());
    }

    [HttpPost]
    public IActionResult CrearUsuario(CrearUsuarioVM usuarioCargado)
    {
        usuarioRep.CreateUsuario(usuarioCargado.UsuarioNuevo);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModificarUsuario(int idUsuarioB)
    {
        ModificarUsuarioVM usuarioM = new ModificarUsuarioVM();
        usuarioM.UsuarioAModificar =
            usuarioRep.GetUsuarioById(idUsuarioB);

        return View(usuarioM);
    }

    [HttpPost]
    public IActionResult ModificarUsuario(ModificarUsuarioVM usuarioM)
    {
        usuarioRep.UpdateUsuario(
            usuarioM.UsuarioAModificar.IdUsuario,
            usuarioM.UsuarioAModificar
        );

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Password(int idUsuario)
    {
        ModificarContraseñaVM contraseñaVM =
            new ModificarContraseñaVM();

        contraseñaVM.IdUsuarioB = idUsuario;
        contraseñaVM.ActualPassword =
            usuarioRep.GetUsuarioById(idUsuario).Password;

        return View(contraseñaVM);
    }

    [HttpPost]
    public IActionResult Password(ModificarContraseñaVM contraseñaVM)
    {
        usuarioRep.ChangePassword(
            contraseñaVM.IdUsuarioB,
            contraseñaVM.newPassword
        );

        return RedirectToAction("Index");
    }

}