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
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de tareas asociadas al tablero");
            return RedirectToAction("Error");
        }
        
    }

    [HttpGet]
    public IActionResult TareasAsignadas(int idTablero)
    {
        try
        {
            var IdSesion = ValidarSesion();
        
            if (!IdSesion.HasValue) return RedirectToAction("Index", "Login");
        
            ListarTareaVM tareaVM = new ListarTareaVM(tareaRep.GetAllTareasByIdTablero(IdSesion.Value),
                                                    usuarioRep.GetAllUsuarios(),
                                                    idTablero);

            var tableroB = tableroRep.GetTableroByIdTablero(idTablero);

            tareaVM.IdCreadorTablero = tableroB.IdUsuarioPropietario;
            tareaVM.NombreTablero = tableroB.Nombre;
            tareaVM.DescripcionTablero = tableroB.Descripcion;

            return RedirectToAction("Index", tareaVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener la lista de tareas asociadas al tablero");
            return RedirectToAction("Error");
        }
        
    }

    [HttpGet]
    public IActionResult AsignarTarea(int idTareaB, int idTablero)
    {
        try
        {
            if (!ValidarCreadorTablero(idTablero)) return RedirectToAction("Index", new { idTablero = idTablero });
        
            AsignarTareaVM asignarVM = new AsignarTareaVM(idTareaB, idTablero);

            asignarVM.Usuarios = usuarioRep.GetAllUsuarios();

            return View(asignarVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al asignar una tarea a un usuario");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult AsignarTarea(AsignarTareaVM asignarTarea)
    {
        if (!ModelState.IsValid)
        {
            asignarTarea.Usuarios = usuarioRep.GetAllUsuarios();

            return View(asignarTarea);
        }
        
        try
        {
            tareaRep.AsignarTarea(asignarTarea.IdUsuarioAsignado, asignarTarea.IdTarea);
            return RedirectToAction("Index", new { idTablero = asignarTarea.IdTablero });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al asignar una tarea a un usuario");
            return RedirectToAction("Error");
        }  
    }

    [HttpGet]
    public IActionResult BorrarAsignarTarea(int idTareaB, int idTablero)
    {
        try
        {
            if (!ValidarCreadorTablero(idTablero)) return RedirectToAction("Index", new { idTablero = idTablero });
            AsignarTareaVM asignarVM = new AsignarTareaVM(idTareaB, idTablero);

            return View(asignarVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al borrar una tarea");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult BorrarAsignarTarea(AsignarTareaVM borrarAsignacion)
    {
        try
        {
            tareaRep.AsignarTarea(0, borrarAsignacion.IdTarea);
            return RedirectToAction("Index", new { idTablero = borrarAsignacion.IdTablero });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al borrar una tarea");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult CrearTarea(int idTableroB)
    {
        try
        {
            if (!ValidarCreadorTablero(idTableroB)) return RedirectToAction("Index", new { idTablero = idTableroB });
        
            CrearTareaVM tareaN = new CrearTareaVM();
            tareaN.NuevaTarea = tareaRep.CreateTarea(idTableroB);

            return View(tareaN);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en crear una tarea");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult CrearTarea(CrearTareaVM tareaNueva)
    {
        try
        {
            tareaRep.CreateTarea(tareaNueva.NuevaTarea);
            return RedirectToAction("Index", new { idTablero = tareaNueva.NuevaTarea.IdTablero });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en crear una tarea");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult ModificarTarea(int idTareaB)
    {
        try
        {
            ModificarTareaVM tareaVM = new ModificarTareaVM { TareaModificar = tareaRep.GetTareaByIdTarea(idTareaB) };
            if (!ValidarCreadorTablero(tareaVM.TareaModificar.IdTablero)) return RedirectToAction("Index", new { idTablero = tareaVM.TareaModificar.IdTablero });
        
            return View(tareaVM);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al modificar la tarea elegida");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult ModificarTarea(ModificarTareaVM tareaVM)
    {
        try
        {
            tareaRep.UpdateTarea(tareaVM.TareaModificar.IdTarea, tareaVM.TareaModificar);
            return RedirectToAction("Index", new { idTablero = tareaVM.TareaModificar.IdTablero });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al modificar la tarea elegida");
            return RedirectToAction("Error");
        }  
    }

    [HttpGet]
    public IActionResult MoverTareaIzquierda(int idTarea)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al desplazar la tarea");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult MoverTareaDerecha(int idTarea)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al desplazar la tarea");
            return RedirectToAction("Error");
        }
    }

    [HttpGet]
    public IActionResult BorrarTarea(int idTareaB)
    {
        try
        {
            var tarea = tareaRep.GetTareaByIdTarea(idTareaB);

            if (!ValidarCreadorTablero(tarea.IdTablero)) return RedirectToAction("Index", new { idTablero = tarea.IdTablero });
        
            if (tarea.Estado != EstadoTarea.Done && tarea.Estado != EstadoTarea.Ideas)
            {
                return RedirectToAction("Index", new { idTablero = tarea.IdTablero });
            }

            return View(tarea);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al borrar la tarea asignada");
            return RedirectToAction("Error");
        }
    }

    [HttpPost]
    public IActionResult BorrarTarea(Tareas tareaBorrar)
    {
        try
        {
            tareaRep.DeleteTarea(tareaBorrar.IdTarea);
            return RedirectToAction("Index", new { idTablero = tareaBorrar.IdTablero });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al borrar la tarea asignada");
            return RedirectToAction("Error");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}