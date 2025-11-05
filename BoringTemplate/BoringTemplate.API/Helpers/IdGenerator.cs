namespace BoringTemplate.API.Helpers;

public static class IdGenerator
{
    public static string Generate(string prefix = "")
    {
        prefix = string.IsNullOrEmpty(prefix) ? prefix : prefix.ToLowerInvariant() + "_";
        return prefix + Guid.NewGuid().ToString("N"); ;
    }

}
