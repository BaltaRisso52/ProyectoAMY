public interface ICloudinaryService
{
    Task<GuardarImagen> SubirImagenAsync(IFormFile archivo);
    void EliminarImagen(string publicId);
}