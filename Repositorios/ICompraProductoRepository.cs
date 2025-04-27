public interface ICompraProductoRepository
{
    void crearCompraProducto(CompraProducto compra);
    List<DetalleCompra> DetalleCompra(string external_reference);
}