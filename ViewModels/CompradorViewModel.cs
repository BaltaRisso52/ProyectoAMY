using System.ComponentModel.DataAnnotations;

public class CompradorViewModel
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres.")]
    public string Apellido { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "El teléfono no es válido.")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "El teléfono debe contener solo 10 dígitos.")]
    public string Telefono { get; set; }
}

public class CompraViewModel
{
    public Carrito Productos { get; set; }
    public CompradorViewModel DatosComprador { get; set; }
}