using Microsoft.AspNetCore.Mvc;

public class CarritoController : Controller
{

    private readonly IProductoRepository productoRepository;

    public CarritoController(IProductoRepository productoRepository)
    {
        this.productoRepository = productoRepository;
    }

    [HttpPost]
    public IActionResult Agregar([FromBody] ProductoCarritoDTO datos)
    {

        try
        {
            var producto = productoRepository.obtenerProductoPorId(datos.ProductoId);
            if (producto is null)
            {
                return Json(new { ok = false });
            }
            var carrito = HttpContext.Session.GetObject<Carrito>("Carrito") ?? new Carrito();

            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == datos.ProductoId);
            if (item != null)
                item.Cantidad++;
            else
                carrito.Items.Add(new CarritoItem
                {
                    ProductoId = producto.IdProducto,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = 1,
                    ImagenUrl = producto.Img
                });

            HttpContext.Session.SetObject("Carrito", carrito);
            return Json(new { ok = true, cantidadTotal = 3 });

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    public IActionResult Index()
    {
        try
        {
            var carrito = HttpContext.Session.GetObject<Carrito>("Carrito") ?? new Carrito();
            var model = new CompraViewModel
            {
                Productos = carrito
            };
            return View(model);

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpPost]
    public IActionResult ActualizarCantidad([FromBody] DatosActualizarCantidad datos)
    {
        try
        {
            var carrito = HttpContext.Session.GetObject<Carrito>("Carrito");
            carrito.ActualizarCantidad(datos.ProductoId, datos.Cantidad);
            HttpContext.Session.SetObject("Carrito", carrito);

            var item = carrito.Items.First(i => i.ProductoId == datos.ProductoId);
            return Json(new
            {
                nuevaCantidad = item.Cantidad,
                subtotal = item.SubTotal,
                total = carrito.Total
            });

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpPost]
    public IActionResult VaciarCarritoAjax()
    {
        try
        {
            HttpContext.Session.Remove("Carrito");
            return Json(new { success = true });

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpPost]
    public IActionResult EliminarProductoAjax(int productoId)
    {

        try
        {
            var carrito = HttpContext.Session.GetObject<Carrito>("Carrito") ?? new Carrito();
            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);

            if (item != null)
            {
                carrito.Items.Remove(item);
                HttpContext.Session.SetObject("Carrito", carrito);
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Producto no encontrado." });

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }
}