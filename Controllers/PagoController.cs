using System.Runtime.CompilerServices;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Mvc;

public class PagoController : Controller
{
    private readonly IConfiguration configuration;
    private readonly ICompraRepository compraRepository;

    public PagoController(IConfiguration configuration, ICompraRepository compraRepository)
    {
        this.configuration = configuration;
        this.compraRepository = compraRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CrearPreferencia(CompraViewModel nuevo)
    {
        try
        {
            // Obtené el carrito desde la sesión (o donde lo guardes)
            var carrito = HttpContext.Session.GetObject<Carrito>("Carrito");

            if (carrito == null || carrito.Items.Count == 0)
                return RedirectToAction("Index", "Carrito");

            // Configurá tu Access Token de prueba (o producción)
            MercadoPagoConfig.AccessToken = Environment.GetEnvironmentVariable("MERCADOPAGO_ACCESS_TOKEN")
        ?? configuration["MercadoPago:AccessToken"];

            // Convertimos cada ítem del carrito a un item de Mercado Pago
            var items = carrito.Items.Select(item => new PreferenceItemRequest
            {
                Id = item.ProductoId.ToString(),
                Title = item.Nombre,
                Quantity = item.Cantidad,
                CurrencyId = "ARS",
                UnitPrice = Convert.ToDecimal(item.Precio)
            }).ToList();

            var id = Guid.NewGuid().ToString();
            var model = nuevo.DatosComprador;

            var preferenceRequest = new PreferenceRequest
            {
                Items = items,
                BackUrls = new PreferenceBackUrlsRequest
                {
                    Success = "https://a660-190-136-59-196.ngrok-free.app/Pago/Exito",
                    Failure = "https://a660-190-136-59-196.ngrok-free.app/Pago/Fallo",
                    Pending = "https://a660-190-136-59-196.ngrok-free.app/Pago/Pendiente",
                },
                AutoReturn = "approved",
                ExternalReference = id,
                Payer = new PreferencePayerRequest
                {
                    Name = model.Nombre,
                    Surname = model.Apellido,
                    Email = model.Email
                },
                NotificationUrl = "https://a660-190-136-59-196.ngrok-free.app/Notificaciones"
            };

            Compra nueva = new Compra
            {
                ExternalReference = id,
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Email = model.Email,
                Telefono = model.Telefono
            };

            compraRepository.crearCompra(nueva);

            var client = new PreferenceClient();
            var preference = await client.CreateAsync(preferenceRequest);

            HttpContext.Session.SetString("preferenceId", preference.Id.ToString());
            return RedirectToAction("ConfirmarPago");

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    public IActionResult ConfirmarPago()
    {
        try
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("preferenceId")))
                return RedirectToAction("Index", "Carrito");

            ViewBag.PreferenceId = HttpContext.Session.GetString("preferenceId");

            return View();

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

    [HttpGet]
    public IActionResult Exito()
    {
        try
        {
            ViewBag.Orden = Request.Query["payment_id"].ToString();
            ViewBag.ExternalReference = Request.Query["external_reference"].ToString();
            return View();

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }
    [HttpGet]
    public IActionResult Fallo() => View();
    [HttpGet]
    public IActionResult Pendiente() => View();
}