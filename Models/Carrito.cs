public class CarritoItem
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; }
    public double Precio { get; set; }
    public int Cantidad { get; set; }
    public double SubTotal => Precio * Cantidad;
    public string ImagenUrl { get; set; } // Opcional
}

public class Carrito
{
    public List<CarritoItem> Items { get; set; } = new List<CarritoItem>();

    public double Total => Items.Sum(i => i.Precio * i.Cantidad);

    public void ActualizarCantidad(int productoId, int nuevaCantidad)
    {
        var item = Items.FirstOrDefault(i => i.ProductoId == productoId);
        if (item != null)
        {
            item.Cantidad = nuevaCantidad;
        }
    }
}

public class DatosActualizarCantidad
{
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
}

public class ProductoCarritoDTO
{
    public int ProductoId { get; set; }
}