namespace org.rsp.management.Tools;

public class RedisKeyGenerator
{
    private static string _prefix = string.Empty;
    public static void SetPrefix(string prefix) => _prefix = !string.IsNullOrWhiteSpace(prefix) ? $"{prefix}:" : string.Empty;
}