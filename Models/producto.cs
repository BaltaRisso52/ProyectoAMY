public class producto
{
    private int idProducto;
    private string nombre;
    private string descripcion;
    private string img;
    private double precio;
    private bool visible;
    private string publicId;

    public int IdProducto { get => idProducto; set => idProducto = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public string Img { get => img; set => img = value; }
    public double Precio { get => precio; set => precio = value; }
    public bool Visible { get => visible; set => visible = value; }
    public string PublicId { get => publicId; set => publicId = value; }
}