public interface IUsuarioRepository
{
    Usuarios? obtenerUsuarioPorId(int id);
    Usuarios? GetUser(string user, string contra);
}