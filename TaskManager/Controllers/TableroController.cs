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
        try{
            var IdPropietario = ValidarSesion();

            if (!IdPropietario.HasValue) return RedirectToAction("Index", "Login");
        
        
            ListarTablerosVM tablerosVM = new ListarTablerosVM(
                                                                tableroRep.GetAllTablerosByIdCreador(IdPropietario.Value),
                                                                IdPropietario.Value
                                                            );
            return View(tablerosVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de tableros del usuario");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult TablerosAsignados()
    {
        try
        {
            var IdPropietario = ValidarSesion();
        
            if (!IdPropietario.HasValue) return RedirectToAction("Index", "Login");
        
            var tablerosAsVM = new ListarTablerosAsVM(tableroRep.GetAllTablerosByIdUsAsignado(IdPropietario.Value));
        
            return View(tablerosAsVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de tableros asignados");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult TablerosOtroUsuario(int idUsuarioB)
    {
        try
        {
            if (!ValidarRol()) return RedirectToAction("Index", "Home");
        
            ListarTablerosVM tablerosVM = new ListarTablerosVM(
                                                                tableroRep.GetAllTableros(),
                                                                idUsuarioB
                                                            );
            return View(tablerosVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de tableros del usuario solicitado");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult CrearTablero()
    {
        try
        {
            var IdCreador = ValidarSesion();
        
            if (!IdCreador.HasValue) return RedirectToAction("Index", "Login");
        
            CrearTableroVM tableroVM = new CrearTableroVM { IdCreador = IdCreador.Value };
            return View(tableroVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear un tablero");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult CrearTablero(CrearTableroVM tableroNuevo)
    {
        var IdCreador = ValidarSesion();
        
        if (!IdCreador.HasValue) return RedirectToAction("Index", "Login");

        if (!ModelState.IsValid)
        {
            tableroNuevo.IdCreador = IdCreador.Value;
            return View(tableroNuevo);
        }

        try
        {
            tableroRep.CreateTablero(tableroNuevo.IdCreador, tableroNuevo.NombreTablero, tableroNuevo.DescripcionTablero);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear un tablero");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult ModificarTablero(int idTableroB)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al modificar el tablero solicitado");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult ModificarTablero(ModificarTableroVM tableroM)
    {
        try
        {
            tableroRep.UpdateTablero(tableroM.IdTablero, tableroM.TableroModificar);
            return RedirectToAction("Index", "Tareas", new { idTablero = tableroM.IdTablero });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al modificar el tablero solicitado");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult CambiarPropietario(int idTableroB)
    {
        try
        {
            if(!ValidarRol()) return RedirectToAction("Index");
        
            CambiarPropietarioVM propietarioVM = new CambiarPropietarioVM(usuarioRep.GetAllUsuarios());
            propietarioVM.TableroCambiar = tableroRep.GetTableroByIdTablero(idTableroB);
        
            return View(propietarioVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al transferir el tablero seleccionado");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult CambiarPropietario(Tablero TableroCambiar)
    {
        try
        {
            tableroRep.UpdateTablero(TableroCambiar.IdTablero, TableroCambiar);
            return RedirectToAction("Index", "Usuarios");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al transferir el tablero seleccionado");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult BorrarTablero(int idTableroB)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al borrar el tablero seleccionado");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult BorrarTablero(Tablero tableroB)
    {
        try
        {
            tableroRep.DeleteTablero(tableroB.IdTablero);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al borrar el tablero seleccionado");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult ErrorBorrarTablero(int idTableroB)
    {
        try
        {
            //Verifico sesion
            var IdSesion = ValidarSesion();
        
            if (!IdSesion.HasValue) return RedirectToAction("Index", "Login");
        
            //Verifico que solo Admin o dueño accedan 
            if (!(ValidarRol() || ValidarCreadorTablero(idTableroB))) return RedirectToAction("Index");
        
            return View(tableroRep.GetTableroByIdTablero(idTableroB));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al borrar el tablero seleccionado");
            return RedirectToAction("Error");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}