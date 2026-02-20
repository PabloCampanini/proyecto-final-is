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

}