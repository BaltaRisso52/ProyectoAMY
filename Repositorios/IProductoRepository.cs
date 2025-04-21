public interface IProductoRepository
{
    void crearProducto(AltaProductoViewModel producto);
    producto obtenerProductoPorId(int id);

    List<producto> BuscarProducto(string produc);
    List<producto> ListarProductos();
    void eliminarProductoPorId(int id);
    void Actualizar(ModificarProductoViewModel produc);
}