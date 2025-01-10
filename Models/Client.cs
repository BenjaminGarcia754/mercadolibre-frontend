using System.ComponentModel.DataAnnotations;

namespace frontendnet.Models;

public class Client
{
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [EmailAddress(ErrorMessage = "El campo {0} debe ser un correo electrónico válido.")]
    [Display(Name = "Correo electrónico")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [MinLength(8, ErrorMessage = "El campo {0} debe tener al menos {1} caracteres.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,13}$",  
        ErrorMessage = "La contraseña debe tener al menos 8 caracteres, máximo 12 caracteres, una mayúscula, una minúscula, un número y un carácter especial.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public required string Password
    { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    [Compare("Password", ErrorMessage = "La confirmación de la contraseña no coincide.")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")]
    public required string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public required string Nombre { get; set; }

    public string Rol { get; set; } = "Usuario";
}