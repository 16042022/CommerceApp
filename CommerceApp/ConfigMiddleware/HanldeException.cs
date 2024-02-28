using System.Net;
using System.Text.Json;

namespace CommerceApp.ConfigMiddleware
{
    public class HanldeException
    {
        private readonly RequestDelegate _next;

        public HanldeException (RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            } catch (Exception ex)
            {
                var respone = context.Response;
                respone.ContentType = "application/json";
                switch (ex)
                {
                    case AggregateException e:
                        respone.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case InvalidDataException e:
                        respone.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case InvalidOperationException e:
                        respone.StatusCode = (int)HttpStatusCode.NotExtended;
                        break;
                    case ArgumentOutOfRangeException e:
                        respone.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        respone.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonSerializer.Serialize(new { message = ex?.Message });
                await respone.WriteAsync(result);
            }
        }
    }

    public static class HandleExceptionMiddleware
    {
        public static IApplicationBuilder HandleException(this IApplicationBuilder app)
        {
            app.UseMiddleware<HanldeException>();
            return app;
        }
    }
}
