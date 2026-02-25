using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;

public class TareasController : ValidacionesController
{
    private readonly ITareaRepository tareaRep;
    private readonly ITableroRepository tableroRep;
    private readonly IUsuarioRepository usuarioRep;
    private readonly ILogger<TareasController> _logger;


    public TareasController(ITareaRepository tareaRepository, ITableroRepository tableroRepository, 
    IUsuarioRepository usuarioRepository, ILogger<TareasController> logger) : base(tableroRepository)
    {
        tareaRep = tareaRepository;
        tableroRep = tableroRepository;
        usuarioRep = usuarioRepository;
        _logger = logger;
    }

    public IActionResult Index(int idTablero)
    {
        var IdSesion = ValidarSesion();

        if (!IdSesion.HasValue) return RedirectToAction("Index", "Login");
        
        ListarTareaVM tareaVM = new ListarTareaVM(tareaRep.GetAllTareasByIdTablero(idTablero),
                                                  usuarioRep.GetAllUsuarios(),
                                                  idTablero);

        var tableroB = tableroRep.GetTableroByIdTablero(idTablero);

        tareaVM.IdCreadorTablero = tableroB.IdUsuarioPropietario;
        tareaVM.NombreTablero = tableroB.Nombre;
        tareaVM.DescripcionTablero = tableroB.Descripcion;

        return View(tareaVM);
        
    }

    [HttpGet]
    public IActionResult TareasAsignadas(int idTablero)
    {
        var IdSesion = ValidarSesion();
    
        if (!IdSesion.HasValue) return RedirectToAction("Index", "Login");
    
        ListarTareaVM tareaVM = new ListarTareaVM(tareaRep.GetAllTareasByIdTablero(idTablero),
                                                  usuarioRep.GetAllUsuarios(),
                                                  idTablero);

        var tableroB = tableroRep.GetTableroByIdTablero(idTablero);

        tareaVM.IdCreadorTablero = tableroB.IdUsuarioPropietario;
        tareaVM.NombreTablero = tableroB.Nombre;
        tareaVM.DescripcionTablero = tableroB.Descripcion;

        return RedirectToAction("Index", tareaVM);
        
    }

    [HttpGet]
    public IActionResult AsignarTarea(int idTareaB, int idTablero)
    {
        if (!ValidarCreadorTablero(idTablero)) return RedirectToAction("Index", new { idTablero = idTablero });
    
        AsignarTareaVM asignarVM = new AsignarTareaVM(idTareaB, idTablero);

        asignarVM.Usuarios = usuarioRep.GetAllUsuarios();

        return View(asignarVM);
    }

    [HttpPost]
    public IActionResult AsignarTarea(AsignarTareaVM asignarTarea)
    {
        
        tareaRep.AsignarTarea(asignarTarea.IdUsuarioAsignado, asignarTarea.IdTarea);
        return RedirectToAction("Index", new { idTablero = asignarTarea.IdTablero });
        
    }

    [HttpGet]
    public IActionResult BorrarAsignarTarea(int idTareaB, int idTablero)
    {
        if (!ValidarCreadorTablero(idTablero)) return RedirectToAction("Index", new { idTablero = idTablero });
        AsignarTareaVM asignarVM = new AsignarTareaVM(idTareaB, idTablero);

        return View(asignarVM);
    }

    [HttpPost]
    public IActionResult BorrarAsignarTarea(AsignarTareaVM borrarAsignacion)
    {
        
        tareaRep.AsignarTarea(0, borrarAsignacion.IdTarea);
        return RedirectToAction("Index", new { idTablero = borrarAsignacion.IdTablero });
    }

    [HttpGet]
    public IActionResult CrearTarea(int idTableroB)
    {
        if (!ValidarCreadorTablero(idTableroB)) return RedirectToAction("Index", new { idTablero = idTableroB });
    
        CrearTareaVM tareaN = new CrearTareaVM();
        tareaN.NuevaTarea = tareaRep.CreateTarea(idTableroB);

        return View(tareaN);
    }

    [HttpPost]
    public IActionResult CrearTarea(CrearTareaVM tareaNueva)
    {
        tareaRep.CreateTarea(tareaNueva.NuevaTarea);
        return RedirectToAction("Index", new { idTablero = tareaNueva.NuevaTarea.IdTablero });
    }

    [HttpGet]
    public IActionResult ModificarTarea(int idTareaB)
    {
        ModificarTareaVM tareaVM = new ModificarTareaVM { TareaModificar = tareaRep.GetTareaByIdTarea(idTareaB) };
        if (!ValidarCreadorTablero(tareaVM.TareaModificar.IdTablero)) return RedirectToAction("Index", new { idTablero = tareaVM.TareaModificar.IdTablero });
    
        return View(tareaVM);
    }

    [HttpPost]
    public IActionResult ModificarTarea(ModificarTareaVM tareaVM)
    {
        tareaRep.UpdateTarea(tareaVM.TareaModificar.IdTarea, tareaVM.TareaModificar);
        return RedirectToAction("Index", new { idTablero = tareaVM.TareaModificar.IdTablero });   
    }

    [HttpGet]
    public IActionResult MoverTareaIzquierda(int idTarea)
    {
        var tarea = tareaRep.GetTareaByIdTarea(idTarea);
        if ((int)tarea.Estado < (int)EstadoTarea.Done)
        {
            tarea.Estado = (EstadoTarea)((int)tarea.Estado - 1);
            switch (tarea.Estado)
            {
                case EstadoTarea.ToDo:
                    tarea.Color = Color.Red;
                    break;

                case EstadoTarea.Doing:
                    tarea.Color = Color.Blue;
                    break;

                case EstadoTarea.Review:
                    tarea.Color = Color.Purple;
                    break;

                case EstadoTarea.Done:
                    tarea.Color = Color.Green;
                    break;
            }
            tareaRep.UpdateTarea(tarea.IdTarea, tarea);
        }
        return RedirectToAction("Index", new { idTablero = tarea.IdTablero });
    }

    [HttpGet]
    public IActionResult MoverTareaDerecha(int idTarea)
    {
        var tarea = tareaRep.GetTareaByIdTarea(idTarea);
        if ((int)tarea.Estado < (int)EstadoTarea.Done)
        {
            tarea.Estado = (EstadoTarea)((int)tarea.Estado + 1);
            switch (tarea.Estado)
            {
                case EstadoTarea.ToDo:
                    tarea.Color = Color.Yellow;
                    break;

                case EstadoTarea.Doing:
                    tarea.Color = Color.Blue;
                    break;

                case EstadoTarea.Review:
                    tarea.Color = Color.Purple;
                    break;

                case EstadoTarea.Done:
                    tarea.Color = Color.Green;
                    break;
            }
            tareaRep.UpdateTarea(tarea.IdTarea, tarea);
        }
        return RedirectToAction("Index", new { idTablero = tarea.IdTablero });
    }

    [HttpGet]
    public IActionResult BorrarTarea(int idTareaB)
    {
        var tarea = tareaRep.GetTareaByIdTarea(idTareaB);

        if (!ValidarCreadorTablero(tarea.IdTablero)) return RedirectToAction("Index", new { idTablero = tarea.IdTablero });
    
        if (tarea.Estado != EstadoTarea.Done && tarea.Estado != EstadoTarea.Ideas)
        {
            return RedirectToAction("Index", new { idTablero = tarea.IdTablero });
        }

        return View(tarea);
    }

    [HttpPost]
    public IActionResult BorrarTarea(Tareas tareaBorrar)
    {
        tareaRep.DeleteTarea(tareaBorrar.IdTarea);
        return RedirectToAction("Index", new { idTablero = tareaBorrar.IdTablero });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}