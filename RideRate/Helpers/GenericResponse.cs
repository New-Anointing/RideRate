using System.Net;

namespace RideRate.Helpers
{
    public class GenericResponse<T>
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
