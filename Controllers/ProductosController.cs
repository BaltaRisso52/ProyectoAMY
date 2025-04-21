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
        try
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                ViewData["EsAdmin"] = true;


            return View();

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpGet]
    public ActionResult AltaProducto()
    {
        try
        {

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Index", "Login");

            ViewData["EsAdmin"] = true;

            return View();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CrearProducto(AltaProductoViewModel producto, IFormFile imagen)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Index", "Login");

            if (imagen != null && imagen.Length > 0)
            {
                var urlImagen = await _cloudinaryService.SubirImagenAsync(imagen);

                producto.Img = urlImagen;

            }
            else
            {
                producto.Img = "default";
            }

            productoRepository.crearProducto(producto);

            return RedirectToAction("Index");

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }

    }

    [HttpGet]
    public ActionResult EliminarProducto(int id)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Index", "Login");


            ViewData["EsAdmin"] = true;

            producto model = productoRepository.obtenerProductoPorId(id);

            if (model is null)
            {
                return NotFound();
            }

            return View(model);

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpPost]
    public ActionResult EliminarProductoOK(int IdProducto)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Index", "Login");


            producto model = productoRepository.obtenerProductoPorId(IdProducto);

            if (model is null)
            {
                return NotFound();
            }

            productoRepository.eliminarProductoPorId(IdProducto);

            return RedirectToAction("Index");

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpGet]
    public ActionResult Detalle(int id)
    {
        try
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                ViewData["EsAdmin"] = true;

            producto model = productoRepository.obtenerProductoPorId(id);

            if (model is null)
            {
                return NotFound();
            }

            return View(model);

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpGet]
    public ActionResult Buscar(string nombre, int pagina = 1, int tamanoPagina = 9)
    {
        try
        {
            bool EsAdmin = false;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
                ViewData["EsAdmin"] = true;
                EsAdmin = true;
            }

            int totalProductos = productoRepository.BuscarProducto(nombre).Count;
            var productos = productoRepository.BuscarProducto(nombre)
                .OrderBy(p => p.Nombre) // Ordenar por nombre, puedes cambiarlo
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            if (!EsAdmin)
            {
                productos = productos.Where(p => p.Visible).ToList();
            }

            ViewData["NombreBuscado"] = nombre;
            ViewData["PaginaActual"] = pagina;
            ViewData["TotalPaginas"] = (int)Math.Ceiling(totalProductos / (double)tamanoPagina);

            return View(productos);

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpGet]
    public ActionResult ListaProductos(int pagina = 1, int tamanoPagina = 9)
    {
        try
        {
            bool EsAdmin = false;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            {
                ViewData["EsAdmin"] = true;
                EsAdmin = true;
            }

            int totalProductos = productoRepository.ListarProductos().Count;
            var productos = productoRepository.ListarProductos()
                .OrderBy(p => p.Nombre) // Ordenar por nombre, puedes cambiarlo
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToList();

            if (!EsAdmin)
            {
                productos = productos.Where(p => p.Visible).ToList();
            }

            ViewData["PaginaActual"] = pagina;
            ViewData["TotalPaginas"] = (int)Math.Ceiling(totalProductos / (double)tamanoPagina);

            return View(productos);

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpPost]
    public IActionResult OcultarProducto(int id)
    {

        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            return RedirectToAction("Index", "Login");

        var producto = productoRepository.obtenerProductoPorId(id);

        if (producto is null)
        {
            return NotFound();
        }

        ModificarProductoViewModel model = new ModificarProductoViewModel
        {
            IdProducto = producto.IdProducto,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio,
            Img = producto.Img,
            Visible = false
        };

        if (!producto.Visible)
        {
            model.Visible = true;
        }

        productoRepository.Actualizar(model); // guardá el cambio en la DB

        return RedirectToAction("Detalle", new { id = producto.IdProducto });
    }

    [HttpGet]
    public IActionResult ModificarProducto(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            return RedirectToAction("Index", "Login");

        var producto = productoRepository.obtenerProductoPorId(id);

        if (producto is null)
        {
            return NotFound();
        }

        ModificarProductoViewModel model = new ModificarProductoViewModel
        {
            IdProducto = producto.IdProducto,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio,
            Img = producto.Img,
            Visible = producto.Visible
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ModificarProductoOK(ModificarProductoViewModel model, IFormFile imagen)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
            return RedirectToAction("Index", "Login");

        if (imagen != null && imagen.Length > 0)
        {
            var urlImagen = await _cloudinaryService.SubirImagenAsync(imagen);

            model.Img = urlImagen;

        }

        productoRepository.Actualizar(model);

        return RedirectToAction("Detalle", new { id = model.IdProducto });
    }
}