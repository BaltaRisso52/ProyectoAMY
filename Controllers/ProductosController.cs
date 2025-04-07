using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller
{

    private readonly IProductoRepository productoRepository;
    private readonly ICloudinaryService _cloudinaryService;

    public ProductosController(IProductoRepository productoRepository, ICloudinaryService cloudinaryService)
    {
        this.productoRepository = productoRepository;
        _cloudinaryService = cloudinaryService;
    }

    public ActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public ActionResult AltaProducto()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CrearProducto(AltaProductoViewModel producto, IFormFile imagen)
    {

        if (imagen != null && imagen.Length > 0)
        {

            var urlImagen = await _cloudinaryService.SubirImagenAsync(imagen);

            producto.Img = urlImagen;

            productoRepository.crearProducto(producto);
        }

        return RedirectToAction("Index");

    }

    [HttpGet]
    public ActionResult EliminarProducto(int id)
    {

        producto model = productoRepository.obtenerProductoPorId(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    public ActionResult EliminarProductoOK(int IdProducto)
    {

        producto model = productoRepository.obtenerProductoPorId(IdProducto);

        if (model is null)
        {
            return NotFound();
        }

        productoRepository.eliminarProductoPorId(IdProducto);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult Detalle(int id)
    {
        producto model = productoRepository.obtenerProductoPorId(id);

        if (model is null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet]
    public ActionResult Buscar(string nombre, int pagina = 1, int tamanoPagina = 9)
    {

        int totalProductos = productoRepository.BuscarProducto(nombre).Count;
        var productos = productoRepository.BuscarProducto(nombre)
            .OrderBy(p => p.Nombre) // Ordenar por nombre, puedes cambiarlo
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToList();

        ViewData["NombreBuscado"] = nombre;
        ViewData["PaginaActual"] = pagina;
        ViewData["TotalPaginas"] = (int)Math.Ceiling(totalProductos / (double)tamanoPagina);

        return View(productos);
    }

    [HttpGet]
    public ActionResult ListaProductos(int pagina = 1, int tamanoPagina = 9)
    {

        int totalProductos = productoRepository.ListarProductos().Count;
        var productos = productoRepository.ListarProductos()
            .OrderBy(p => p.Nombre) // Ordenar por nombre, puedes cambiarlo
            .Skip((pagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToList();

        ViewData["PaginaActual"] = pagina;
        ViewData["TotalPaginas"] = (int)Math.Ceiling(totalProductos / (double)tamanoPagina);

        return View(productos);
    }
}