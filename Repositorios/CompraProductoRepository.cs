using Npgsql;

public class CompraProductoRepository : ICompraProductoRepository
{

    private readonly string _ConnectionString;

    public CompraProductoRepository(string connectionString)
    {
        _ConnectionString = connectionString;
    }

    public void crearCompraProducto(CompraProducto compra)
    {

        string consulta = @"INSERT INTO compra_productos (id_compra, id_producto, cantidad) VALUES (@id_compra, @id_producto, @cantidad);";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);

            command.Parameters.AddWithValue("@id_compra", compra.CompraId);
            command.Parameters.AddWithValue("@id_producto", compra.ProductoId);
            command.Parameters.AddWithValue("@cantidad", compra.Cantidad);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public List<DetalleCompra> DetalleCompra(string external_reference)
    {
        List<DetalleCompra> productos = new();

        string consulta = @"
    SELECT 
        p.id,
        p.nombre,
        p.descripcion,
        p.precio,
        p.img,
        c.cantidad
    FROM 
        producto p
    JOIN 
        compra_productos c ON c.id_producto = p.id
    WHERE 
        c.id_compra = @id;
";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);
            command.Parameters.AddWithValue("@id", external_reference);

            using (var reader = command.ExecuteReader())
            {

                while (reader.Read())
                {
                    DetalleCompra producto = new();
                    producto.Producto = new producto();

                    producto.Producto.IdProducto = Convert.ToInt32(reader["id"]);
                    producto.Producto.Nombre = reader["nombre"].ToString();
                    producto.Producto.Descripcion = reader["descripcion"].ToString();
                    producto.Producto.Precio = Convert.ToDouble(reader["precio"]);
                    producto.Producto.Img = reader["img"].ToString();
                    producto.Cantidad = Convert.ToInt32(reader["cantidad"]);

                    productos.Add(producto);
                }
            }

            connection.Close();
        }

        return productos;
    }
}