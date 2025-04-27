using Microsoft.AspNetCore.Mvc;

public class ComprasController : Controller
{
    private readonly ICompraRepository compraRepository;
    private readonly ICompraProductoRepository compraProductoRepository;

    public ComprasController(ICompraRepository compraRepository, ICompraProductoRepository compraProductoRepository)
    {
        this.compraRepository = compraRepository;
        this.compraProductoRepository = compraProductoRepository;
    }

    public IActionResult Index()
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Index", "Login");

            ViewData["EsAdmin"] = true;

            var compras = compraRepository.ListarCompras();
            return View(compras);

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    public IActionResult Detalle(string id)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Index", "Login");

            ViewData["EsAdmin"] = true;

            List<DetalleCompra> compra = compraProductoRepository.DetalleCompra(id); // Incluye productos
            if (compra == null) return NotFound();
            return View(compra);

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpPost]
    public IActionResult ActualizarEstado(string externalreference, string nuevoEstado)
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User")))
                return RedirectToAction("Index", "Login");

            ViewData["EsAdmin"] = true;

            compraRepository.ActualizarEstado(externalreference, nuevoEstado);
            return RedirectToAction("Index");

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }
}