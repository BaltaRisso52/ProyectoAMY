using Npgsql;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly string _ConnectionString;

    public UsuarioRepository(string connectionString)
    {
        _ConnectionString = connectionString;
    }

    public Usuarios? obtenerUsuarioPorId(int id)
    {
        Usuarios usuario = null;

        string consulta = @"SELECT * FROM usuario WHERE id = @id;";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);

            command.Parameters.AddWithValue("@id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario = new Usuarios();

                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreUsuario = reader["nombre_usuario"].ToString();
                    usuario.Password = reader["password"].ToString();
                }
            }

            connection.Close();
        }

        return usuario;
    }

    public Usuarios? GetUser(string user, string contra)
    {
        Usuarios usuario = null;

        string consulta = @"SELECT * FROM usuario WHERE nombre_usuario = @usuario AND password = @contra;";

        using (var connection = new NpgsqlConnection(_ConnectionString))
        {
            connection.Open();

            var command = new NpgsqlCommand(consulta, connection);

            command.Parameters.AddWithValue("@usuario", user);
            command.Parameters.AddWithValue("@contra", contra);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario = new Usuarios();

                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.NombreUsuario = reader["nombre_usuario"].ToString();
                    usuario.Password = reader["password"].ToString();
                }
            }
            connection.Close();
        }

        return usuario;
    }
}