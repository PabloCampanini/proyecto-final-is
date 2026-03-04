public class Tareas
{
    private int idTarea;
    private int idTablero;
    private string nombre;
    private EstadoTarea estado;
    private string descripcion;
    private Color color;
    private int? idUsuarioAsignado;

    public Tareas()
    {
    }

    public int IdTarea { get => idTarea; set => idTarea = value; }
    public int IdTablero { get => idTablero; set => idTablero = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public EstadoTarea Estado { get => estado; set => estado = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public Color Color { get => color; set => color = value; }
    public int? IdUsuarioAsignado { get => idUsuarioAsignado; set => idUsuarioAsignado = value; }
}