public class Compra
{
    public string ExternalReference { get; set; }

    public string PaymentId { get; set; }

    public string Status { get; set; }

    public decimal? MontoTotal { get; set; }

    public DateTime? DateApproved { get; set; }

    public string Nombre { get; set; }

    public string Apellido { get; set; }

    public string Email { get; set; }

    public string Telefono { get; set; }
    public string EstadoVisible { get; set; }
    public List<CompraProducto> CompraProductos { get; set; }
}