using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller
{

    private readonly IProductoRepository productoRepository;

    public ProductosController(IProductoRepository productoRepository)
    {
        this.productoRepository = productoRepository;
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
    public ActionResult CrearProducto(AltaProductoViewModel producto, IFormFile imagen)
    {

        if (imagen != null && imagen.Length > 0)
        {
            // Generar un nombre único para la imagen
            var fileName = Path.GetFileName(imagen.FileName);
            var fileExtension = Path.GetExtension(imagen.FileName);
            var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

            // Definir la ruta donde se guardará la imagen
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", uniqueFileName);

            // Guardar la imagen en el servidor
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imagen.CopyTo(fileStream);
            }

            // Guardar el nombre de la imagen en la base de datos
            producto.Img = uniqueFileName;

            // Aquí agregarías el código para guardar el producto en la base de datos
            // por ejemplo:
            // _productoRepository.AgregarProducto(producto);

            productoRepository.crearProducto(producto);

            TempData["Mensaje"] = "Producto creado con éxito";

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