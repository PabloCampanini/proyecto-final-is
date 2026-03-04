using Microsoft.AspNetCore.Mvc;

public class ValidacionesController : Controller
{
    private readonly ITableroRepository tableroRep;

    public ValidacionesController(ITableroRepository tableroRepository)
    {
        tableroRep = tableroRepository;
    }
    protected bool ValidarRol()
    {
        return HttpContext.Session.GetString("rolSesion") == "Administrador" ? true : false;
    }

    protected int? ValidarSesion()
    {
        var id = HttpContext.Session.GetInt32("idSesion");
        return id.HasValue ? id.Value : null;
    }
    protected bool ValidarCreadorTablero(int idTableroB)
    {
        var tableroB = tableroRep.GetTableroByIdTablero(idTableroB);
        var IdSesion = ValidarSesion();

        if (!IdSesion.HasValue)
        {
            return false;
        }

        return  IdSesion.Value == tableroB.IdUsuarioPropietario ? true : false;
    }
}