using System.Text.Json;

public static class SessionExtensions
{
    public static void SetObject<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value)); // Guarda como JSON
    }

    public static T GetObject<T>(this ISession session, string key)
    {
        var value = session.GetString(key); // Lee el string JSON
        return value == null ? default : JsonSerializer.Deserialize<T>(value); // Lo convierte de nuevo al objeto
    }
}