using BookStoreAPI.Shared;
using Newtonsoft.Json;
using System.Net;
using System;

namespace BookStoreAPI.Middleware
{
    public class GlobalErrorHandler
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var response = new GenericResponse<object>();
            context.Response.ContentType = "application/json";
            try
            {
                await _next(context);
            }

            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                switch (ex)
                {
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = HttpStatusCode.NotFound;
                        break; 
                    default:
                        // unhandled error
                        response.StatusCode = context.Response.StatusCode == (int)HttpStatusCode.OK ? HttpStatusCode.InternalServerError : (HttpStatusCode)context.Response.StatusCode;
                        break;
                }


                context.Response.ContentType = "application/json";
                var result = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(result);
            }

        }
    }
}
