public class ListarUsuariosVM
{
    public List<Usuarios> Administradores { get; set; }
    public List<Usuarios> Operadores { get; set; }

    public ListarUsuariosVM(List<Usuarios> ListaUsuarios)
    {
        Administradores = ListaUsuarios.Where(u => u.Rol == RolUsuario.Administrador).ToList();
        Operadores = ListaUsuarios.Where(u => u.Rol == RolUsuario.Operador).ToList();
    }
}