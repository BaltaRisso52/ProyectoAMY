using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

public class ModificarProductoViewModel
{
    [Required]
    public int IdProducto { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; }
    [Required(ErrorMessage = "La descripcion es obligatoria.")]
    [StringLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres.")]
    public string Descripcion { get; set; }
    [ValidateNever]
    public string Img { get; set; }
    [ValidateNever]
    public bool Visible { get; set; }
    [Required(ErrorMessage = "El precio es obligatorio.")]
    public double Precio { get; set; }
}