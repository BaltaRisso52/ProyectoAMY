using System.Text.Json;
using MercadoPago.Client.MerchantOrder;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Resource.MerchantOrder;
using Microsoft.AspNetCore.Mvc;

[Route("Notificaciones")]
[ApiController]
public class NotificacionesController : ControllerBase
{

    private readonly IConfiguration configuration;
    private readonly ICompraRepository compraRepository;
    private readonly ICompraProductoRepository compraProductoRepository;

    public NotificacionesController(IConfiguration configuration, ICompraRepository compraRepository, ICompraProductoRepository compraProductoRepository)
    {
        this.configuration = configuration;
        this.compraRepository = compraRepository;
        this.compraProductoRepository = compraProductoRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Notificaciones([FromBody] JsonElement body)
    {

        try
        {
            Console.WriteLine(body.ToString());

            if (body.TryGetProperty("type", out var type) && type.GetString() == "payment")
            {
                if (body.TryGetProperty("data", out var data))
                {

                    var client = new PaymentClient();
                    var paymentId = long.Parse(data.GetProperty("id").ToString());
                    var payment = await client.GetAsync(paymentId);

                    var compra = new Compra
                    {
                        ExternalReference = payment.ExternalReference,
                        PaymentId = payment.Id.ToString(),
                        Status = payment.Status,
                        MontoTotal = payment.TransactionAmount,
                        DateApproved = payment.DateApproved
                    };

                    foreach (var item in payment.AdditionalInfo.Items)
                    {
                        var CompraProducto = new CompraProducto
                        {
                            CompraId = payment.ExternalReference,
                            ProductoId = Convert.ToInt32(item.Id),
                            Cantidad = item.Quantity ?? 0
                        };

                        compraProductoRepository.crearCompraProducto(CompraProducto);
                    }

                    compraRepository.Actualizar(compra);


                    string json = JsonSerializer.Serialize(payment, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });
                    Console.WriteLine(json);
                }
            }


            return Ok();

        }
        catch (Exception ex)
        {
            return StatusCode(500, "Ocurrió un error inesperado en el servidor.");
        }
    }

}