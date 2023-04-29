using System.Text.Json;

namespace API.Helpers
{
    //Note: For all extension methods class will be static
    public static class HTTPExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
            var jsonOptions = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            response.Headers.Add("Pagination", JsonSerializer.Serialize(header,jsonOptions));

            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
        }
    }
}