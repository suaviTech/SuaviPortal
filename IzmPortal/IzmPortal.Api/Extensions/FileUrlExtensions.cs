namespace IzmPortal.Api.Extensions;

public static class FileUrlExtensions
{
    public static string ToPublicUrl(
        this string relativePath,
        HttpRequest request)
    {
        return $"{request.Scheme}://{request.Host}/{relativePath}";
    }
}
