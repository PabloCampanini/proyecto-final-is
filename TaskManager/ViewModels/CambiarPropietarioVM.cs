public class CambiarPropietarioVM
{
    public Tablero TableroCambiar {get;set;} = new Tablero();
    public ListarUsuariosVM UsuariosVM {get;set;}

    public CambiarPropietarioVM(List<Usuarios> ListaUsuarios)
    {
        UsuariosVM = new ListarUsuariosVM(ListaUsuarios);
    }
    
}