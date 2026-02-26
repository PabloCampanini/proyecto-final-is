using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;

public class TableroController : ValidacionesController
{
    private readonly ITableroRepository tableroRep;
    private readonly ITareaRepository tareaRep;
    private readonly IUsuarioRepository usuarioRep;
    private readonly ILogger<TableroController> _logger;

    public TableroController(ITableroRepository tableroRepository, ITareaRepository tareaRepository,
    IUsuarioRepository usuarioRepository, ILogger<TableroController> logger) : base(tableroRepository)
    {
        tableroRep = tableroRepository;
        tareaRep = tareaRepository;
        usuarioRep = usuarioRepository;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var IdPropietario = ValidarSesion();

        if (!IdPropietario.HasValue) return RedirectToAction("Index", "Login");
    
    
        ListarTablerosVM tablerosVM = new ListarTablerosVM(
                                                            tableroRep.GetAllTablerosByIdCreador(IdPropietario.Value),
                                                            IdPropietario.Value
                                                          );
        return View(tablerosVM);
    }

    [HttpGet]
    public IActionResult TablerosAsignados()
    {
        var IdPropietario = ValidarSesion();
    
        if (!IdPropietario.HasValue) return RedirectToAction("Index", "Login");
    
        var tablerosAsVM = new ListarTablerosAsVM(tableroRep.GetAllTablerosByIdUsAsignado(IdPropietario.Value));
    
        return View(tablerosAsVM);
    }

    [HttpGet]
    public IActionResult TablerosOtroUsuario(int idUsuarioB)
    {
        if (!ValidarRol()) return RedirectToAction("Index", "Home");
    
        ListarTablerosVM tablerosVM = new ListarTablerosVM(
                                                            tableroRep.GetAllTableros(),
                                                            idUsuarioB
                                                          );
        return View(tablerosVM);
    }

    [HttpGet]
    public IActionResult CrearTablero()
    {
        var IdCreador = ValidarSesion();
    
        if (!IdCreador.HasValue) return RedirectToAction("Index", "Login");
    
        CrearTableroVM tableroVM = new CrearTableroVM { IdCreador = IdCreador.Value };
        return View(tableroVM);
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
        var IdSesion = ValidarSesion();
    
        if (!IdSesion.HasValue) return RedirectToAction("Index", "Login");
    
        ModificarTableroVM tableroVM = new ModificarTableroVM
        {
            IdTablero = idTableroB,
            TableroModificar = tableroRep.GetTableroByIdTablero(idTableroB)
        };
    
        if (!ValidarCreadorTablero(idTableroB)) return RedirectToAction("Index");
    
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
        if(!ValidarRol()) return RedirectToAction("Index");
    
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
        //Verifico sesion
        var IdSesion = ValidarSesion();
    
        if (!IdSesion.HasValue) return RedirectToAction("Index", "Login");
    
        //Verifico que solo Admin o dueño borre tablero
        if (!(ValidarRol() || ValidarCreadorTablero(idTableroB))) return RedirectToAction("Index");
    
        //Verifico que no tenga tareas por resolver
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
        //Verifico sesion
        var IdSesion = ValidarSesion();
    
        if (!IdSesion.HasValue) return RedirectToAction("Index", "Login");
    
        //Verifico que solo Admin o dueño accedan 
        if (!(ValidarRol() || ValidarCreadorTablero(idTableroB))) return RedirectToAction("Index");
    
        return View(tableroRep.GetTableroByIdTablero(idTableroB));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}