using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(Cloudinary cloudinary)
    {
        _cloudinary = cloudinary;
    }

    public async Task<GuardarImagen> SubirImagenAsync(IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation()
            .AspectRatio("1:1")
            .Crop("pad")
            .Background("white"),
            PublicId = "producto_" + Guid.NewGuid().ToString()
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        GuardarImagen nueva = new GuardarImagen
        {
            Url = uploadResult.SecureUrl.ToString(),
            PublicId = uploadResult.PublicId
        };

        return nueva;
    }

    public void EliminarImagen(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var resultado = _cloudinary.Destroy(deleteParams); // Elimina la imagen de Cloudinary

        if (resultado.StatusCode != System.Net.HttpStatusCode.OK)
        {
            throw new Exception("Error al eliminar la imagen de Cloudinary");
        }
    }
}

public class GuardarImagen
{
    public string Url { get; set; }
    public string PublicId { get; set; }
}