namespace IzmPortal.Application.Common;

public static class FileUrlBuilder
{
    public static string Build(
        string baseUrl,
        string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return string.Empty;

        return $"{baseUrl.TrimEnd('/')}/{relativePath.TrimStart('/')}";
    }
}
