public class ListarTareaVM
{
    public int IdTablero { get; set; }
    public int IdCreadorTablero { get; set; }
    public string NombreTablero { get; set; }
    public string DescripcionTablero { get; set; }
    public List<Usuarios> Usuarios { get; set; }
    public List<Tareas> TareasIdeas { get; set; }
    public List<Tareas> TareasPorHacer { get; set; }
    public List<Tareas> TareasEnProceso { get; set; }
    public List<Tareas> TareasEnRevision { get; set; }
    public List<Tareas> TareasCompletadas { get; set; }

    public ListarTareaVM(List<Tareas> ListaTareas, List<Usuarios> ListaUsuarios, int IdTableroOrigen)
    {
        IdTablero = IdTableroOrigen;
        Usuarios = ListaUsuarios;
        TareasIdeas = ListaTareas.Where(t => t.Estado == EstadoTarea.Ideas).ToList();
        TareasPorHacer = ListaTareas.Where(t => t.Estado == EstadoTarea.ToDo).ToList();
        TareasEnProceso = ListaTareas.Where(t => t.Estado == EstadoTarea.Doing).ToList();
        TareasEnRevision = ListaTareas.Where(t => t.Estado == EstadoTarea.Review).ToList();
        TareasCompletadas = ListaTareas.Where(t => t.Estado == EstadoTarea.Done).ToList();
    }
}