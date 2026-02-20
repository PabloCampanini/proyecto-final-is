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

}