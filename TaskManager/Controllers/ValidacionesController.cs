using Microsoft.AspNetCore.Mvc;

public class ValidacionesController : Controller
{
    protected bool ValidarRol()
    {
        return HttpContext.Session.GetString("rolSesion") == "Administrador" ? true : false;
    }

    protected int? ValidarSesion()
    {
        var id = HttpContext.Session.GetInt32("idSesion");
        return id.HasValue ? id.Value : null;
    }
}