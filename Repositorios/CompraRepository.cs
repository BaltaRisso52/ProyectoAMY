using Npgsql;

public class CompraRepository : ICompraRepository
{

    private readonly string _ConnectionString;

    public CompraRepository(string connectionString)
    {
        _ConnectionString = connectionString;
    }

    public void crearCompra(Compra compra)
    {

        string consulta = @"INSERT INTO compras (external_reference, nombre, apellido, email, telefono) VALUES (@id, @nombre, @apellido, @email, @tel);";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);

            command.Parameters.AddWithValue("@id", compra.ExternalReference);
            command.Parameters.AddWithValue("@nombre", compra.Nombre);
            command.Parameters.AddWithValue("@apellido", compra.Apellido);
            command.Parameters.AddWithValue("@email", compra.Email);
            command.Parameters.AddWithValue("@tel", compra.Telefono);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void Actualizar(Compra compra)
    {

        string consulta = @"UPDATE compras SET payment_id = @paymentid, estado = @estado, monto_total = @monto, fecha_aprobada = @fecha WHERE external_reference = @id;";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);

            command.Parameters.AddWithValue("@paymentid", compra.PaymentId);
            command.Parameters.AddWithValue("@estado", compra.Status);
            command.Parameters.AddWithValue("@monto", compra.MontoTotal);
            command.Parameters.AddWithValue("@fecha", compra.DateApproved);
            command.Parameters.AddWithValue("@id", compra.ExternalReference);

            command.ExecuteNonQuery();

            connection.Close();
        }

    }

    public List<Compra> ListarCompras()
    {

        List<Compra> compras = new();

        string consulta = @"SELECT * FROM compras WHERE payment_id IS NOT NULL ORDER BY external_reference, fecha_creacion ASC";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);

            using (var reader = command.ExecuteReader())
            {

                while (reader.Read())
                {
                    Compra compra = new();

                    compra.ExternalReference = reader["external_reference"].ToString();
                    compra.PaymentId = reader["payment_id"].ToString();
                    compra.Status = reader["estado"].ToString();
                    compra.MontoTotal = Convert.ToDecimal(reader["monto_total"]);
                    compra.DateApproved = DateTime.Parse(reader["fecha_aprobada"].ToString());
                    compra.Nombre = reader["nombre"].ToString();
                    compra.Apellido = reader["apellido"].ToString();
                    compra.Email = reader["email"].ToString();
                    compra.Telefono = reader["telefono"].ToString();
                    compra.EstadoVisible = reader["estado_vista"].ToString();

                    compras.Add(compra);
                }
            }

            connection.Close();
        }

        return compras;
    }

    public void ActualizarEstado(string externalreference, string nuevoEstado)
    {
        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();
            var query = "UPDATE compras SET estado_vista = @estado WHERE external_reference = @id";
            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("estado", nuevoEstado);
                command.Parameters.AddWithValue("id", externalreference);
                command.ExecuteNonQuery();
            }
        }
    }
}