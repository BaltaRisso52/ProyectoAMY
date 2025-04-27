public class CompraProducto
{
    public int Id { get; set; }
    public string CompraId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
}

public class DetalleCompra
{
    public producto Producto { get; set; }
    public int Cantidad { get; set; }
}