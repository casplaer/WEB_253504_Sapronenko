namespace WEB_253504_Sapronenko.UI.Extensions
{
    public static class HttpRequestExtensions
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
