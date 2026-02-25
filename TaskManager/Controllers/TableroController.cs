using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;

public class TableroController : Controller
{
    private readonly ITableroRepository tableroRep;
    private readonly ITareaRepository tareaRep;
    private readonly IUsuarioRepository usuarioRep;

    public TableroController(ITableroRepository tableroRepository, ITareaRepository tareaRepository,
    IUsuarioRepository usuarioRepository)
    {
        tableroRep = tableroRepository;
        tareaRep = tareaRepository;
        usuarioRep = usuarioRepository;
    }

    public IActionResult Index()
    {
        //Modificar cuando se tenga el logueo
        var IdPropietario = 0;
    
        ListarTablerosVM tablerosVM = new ListarTablerosVM(
                                                            tableroRep.GetAllTableros(),
                                                            IdPropietario
                                                          );
        return View(tablerosVM);
    }

    [HttpGet]
    public IActionResult TablerosAsignados()
    {
        //Modificar cuando se tenga el logueo
        var tablerosAsVM = new ListarTablerosAsVM(tableroRep.GetAllTableros());
    
        return View(tablerosAsVM);
    }

    [HttpGet]
    public IActionResult TablerosOtroUsuario(int idUsuarioB)
    {
        ListarTablerosVM tablerosVM = new ListarTablerosVM(
                                                            tableroRep.GetAllTableros(),
                                                            idUsuarioB
                                                          );
        return View(tablerosVM);
    }

    [HttpGet]
    public IActionResult CrearTablero()
    {
        //Modificar cuando se tenga el logueo
        return View(new CrearTableroVM());
    }

    [HttpPost]
    public IActionResult CrearTablero(CrearTableroVM tableroNuevo)
    {
        tableroRep.CreateTablero(tableroNuevo.IdCreador, tableroNuevo.NombreTablero, tableroNuevo.DescripcionTablero);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ModificarTablero(int idTableroB)
    {
        //Modificar cuando se tenga el logueo
        ModificarTableroVM tableroVM = new ModificarTableroVM();
        tableroVM.IdTablero = idTableroB;
        tableroVM.TableroModificar = tableroRep.GetTableroByIdTablero(idTableroB);
    
        return View(tableroVM);
    }

    [HttpPost]
    public IActionResult ModificarTablero(ModificarTableroVM tableroM)
    {
        tableroRep.UpdateTablero(tableroM.IdTablero, tableroM.TableroModificar);
        return RedirectToAction("Index", "Tareas", new { idTablero = tableroM.IdTablero });
    }

    [HttpGet]
    public IActionResult CambiarPropietario(int idTableroB)
    {
    
        CambiarPropietarioVM propietarioVM = new CambiarPropietarioVM(usuarioRep.GetAllUsuarios());
        propietarioVM.TableroCambiar = tableroRep.GetTableroByIdTablero(idTableroB);
    
        return View(propietarioVM);
    }

    [HttpPost]
    public IActionResult CambiarPropietario(Tablero TableroCambiar)
    {
        tableroRep.UpdateTablero(TableroCambiar.IdTablero, TableroCambiar);
        return RedirectToAction("Index", "Usuarios");
    }

    [HttpGet]
    public IActionResult BorrarTablero(int idTableroB)
    {
        //Modificar cuando se tenga el logueo
        int cantidadTareas = tareaRep.GetAllTareasByIdTablero(idTableroB).Count;
    
        if (cantidadTareas != 0) return RedirectToAction("ErrorBorrarTablero", new { idTableroB = idTableroB });

        return View(tableroRep.GetTableroByIdTablero(idTableroB));
    }

    [HttpPost]
    public IActionResult BorrarTablero(Tablero tableroB)
    {
        tableroRep.DeleteTablero(tableroB.IdTablero);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ErrorBorrarTablero(int idTableroB)
    {
        return View(tableroRep.GetTableroByIdTablero(idTableroB));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}