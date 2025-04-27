public class MercadoPagoWebhook
{
    public string Action { get; set; }
    public string ApiVersion { get; set; }
    public WebhookData Data { get; set; }
    public DateTime DateCreated { get; set; }
    public string Id { get; set; }
    public bool LiveMode { get; set; }
    public string Type { get; set; }
    public long UserId { get; set; }
}

public class WebhookData
{
    public string Id { get; set; }
}