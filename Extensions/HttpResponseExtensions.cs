

internal static partial class HttpResponseExtensions
{
    public static async Task WriteJsonAsync(this HttpResponse response, string json, string contentType = null)
    {
        response.ContentType = (contentType ?? "application/json; charset=UTF-8");
        await response.WriteAsync(json);
        await response.Body.FlushAsync();
    }
}