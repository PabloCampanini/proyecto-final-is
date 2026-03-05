using System.ComponentModel.DataAnnotations;

public class LoginVM
{
    [Required(ErrorMessage = "Debe ingresar un Email válido.")]
    public string Email {get;set;}
    
    [Required(ErrorMessage = "Debe ingresar una contraseña.")]
    public string Password {get;set;}
    public string? ErrorMessage { get; set; }
}