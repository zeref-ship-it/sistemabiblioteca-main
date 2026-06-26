using System.Text.Json;

namespace sistemabiblioteca.Helpers;

/// <summary>
/// Métodos utilitários para guardar/recuperar objetos complexos na sessão
/// usando serialização JSON.
/// </summary>
public static class SessionExtensions
{
    public static void SetObject<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T? GetObject<T>(this ISession session, string key)
    {
        var data = session.GetString(key);
        return data is null ? default : JsonSerializer.Deserialize<T>(data);
    }
}
