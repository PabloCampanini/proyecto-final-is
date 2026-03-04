public class ListarTablerosVM
{
    public List<Tablero> TablerosUsuario { get; set; }
    public int Propietario { get; set; }

    public ListarTablerosVM(List<Tablero> ListaTableros, int IdPropietario)
    {
        TablerosUsuario = ListaTableros;
        Propietario = IdPropietario;
    }
}