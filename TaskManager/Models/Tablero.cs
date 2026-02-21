public class Tablero
{
    private int idTablero;
    private int idUsuarioPropietario;
    private string nombre;
    private string descripcion;
    
    public Tablero()
    {
    }

    public int IdTablero { get => idTablero; set => idTablero = value; }
    public int IdUsuarioPropietario { get => idUsuarioPropietario; set => idUsuarioPropietario = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
}