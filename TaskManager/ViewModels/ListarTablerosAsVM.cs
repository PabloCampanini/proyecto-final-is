public class ListarTablerosAsVM
{
    public List<Tablero> TablerosAsignados { get; set; }

    public ListarTablerosAsVM(List<Tablero> ListaTableros)
    {
        TablerosAsignados = ListaTableros;
    }
}