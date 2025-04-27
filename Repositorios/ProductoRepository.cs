using Npgsql;

public class ProductoRepository : IProductoRepository
{

    private readonly string _ConnectionString;

    public ProductoRepository(string connectionString)
    {
        _ConnectionString = connectionString;
    }
    
    public void crearProducto(AltaProductoViewModel producto){
        
        string consulta = @"INSERT INTO producto (nombre, precio,descripcion, img, public_id) VALUES (@nombre, @precio, @descripcion, @img, @public_id);";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);

            command.Parameters.AddWithValue("@nombre", producto.Nombre);
            command.Parameters.AddWithValue("@precio", producto.Precio);
            command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@img", producto.Img);
            command.Parameters.AddWithValue("@public_id", producto.PublicId);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public producto obtenerProductoPorId(int id){

        producto producto = null;

        string consulta = @"SELECT * FROM producto WHERE id = @id;";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);

            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    producto = new producto();

                    producto.IdProducto = Convert.ToInt32(reader["id"]);
                    producto.Nombre = reader["nombre"].ToString();
                    producto.Descripcion = reader["descripcion"].ToString();
                    producto.Precio = Convert.ToDouble(reader["precio"]);
                    producto.Img = reader["img"].ToString();
                    producto.Visible = Convert.ToBoolean(reader["visible"]);
                    producto.PublicId = reader["public_id"].ToString();

                }
            }

            connection.Close();
        }

        return producto;
    }

    public List<producto> BuscarProducto(string produc){

        List<producto> productos = new();

        string consulta = @"SELECT * FROM producto WHERE nombre ILIKE @producto;";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);

            command.Parameters.AddWithValue("@producto", $"%{produc}%");

            using (var reader = command.ExecuteReader())
            {

                while (reader.Read())
                {
                    producto producto = new();

                    producto.IdProducto = Convert.ToInt32(reader["id"]);
                    producto.Nombre = reader["nombre"].ToString();
                    producto.Descripcion = reader["descripcion"].ToString();
                    producto.Precio = Convert.ToDouble(reader["precio"]);
                    producto.Img = reader["img"].ToString();
                    producto.Visible = Convert.ToBoolean(reader["visible"]);
                    producto.PublicId = reader["public_id"].ToString();

                    productos.Add(producto);
                }
            }

            connection.Close();
        }

        return productos;
    }

    public List<producto> ListarProductos(){

        List<producto> productos = new();

        string consulta = @"SELECT * FROM producto";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);

            using (var reader = command.ExecuteReader())
            {

                while (reader.Read())
                {
                    producto producto = new();

                    producto.IdProducto = Convert.ToInt32(reader["id"]);
                    producto.Nombre = reader["nombre"].ToString();
                    producto.Descripcion = reader["descripcion"].ToString();
                    producto.Precio = Convert.ToDouble(reader["precio"]);
                    producto.Img = reader["img"].ToString();
                    producto.Visible = Convert.ToBoolean(reader["visible"]);
                    producto.PublicId = reader["public_id"].ToString();

                    productos.Add(producto);
                }
            }

            connection.Close();
        }

        return productos;
    }

    public void eliminarProductoPorId(int id)
    {

        string consulta = @"DELETE FROM producto WHERE id = @id;";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void Actualizar(ModificarProductoViewModel produc){

        string consulta = @"UPDATE producto SET nombre = @nombre, descripcion = @descripcion, precio = @precio, img = @img, visible = @visible, public_id = @public_id WHERE id = @id;";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);

            command.Parameters.AddWithValue("@nombre", produc.Nombre);
            command.Parameters.AddWithValue("@descripcion", produc.Descripcion);
            command.Parameters.AddWithValue("@precio", produc.Precio);
            command.Parameters.AddWithValue("@img", produc.Img);
            command.Parameters.AddWithValue("@visible", produc.Visible);
            command.Parameters.AddWithValue("@public_id", produc.PublicId);
            command.Parameters.AddWithValue("@id", produc.IdProducto);

            command.ExecuteNonQuery();

            connection.Close();
        }

    }

}