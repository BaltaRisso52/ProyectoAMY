public interface ICompraRepository
{
    void crearCompra(Compra compra);
    void Actualizar(Compra compra);
    List<Compra> ListarCompras();
    void ActualizarEstado(string externalreference, string nuevoEstado);
}