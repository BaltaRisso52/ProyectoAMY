public interface ICloudinaryService
{
    Task<string> SubirImagenAsync(IFormFile archivo);
}