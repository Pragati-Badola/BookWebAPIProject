using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using System.Net;

namespace BookStoreAPI.Shared
{
    public class Response : ObjectResultExecutor
    {
        public Response(OutputFormatterSelector formatterSelector, IHttpResponseStreamWriterFactory writerFactory, ILoggerFactory loggerFactory, IOptions<MvcOptions> mvcOptions) : base(formatterSelector, writerFactory, loggerFactory, mvcOptions)
        {
        }

        public override Task ExecuteAsync(ActionContext context, ObjectResult result)
        {
            var response = new GenericResponse<object>();
            int statusCodeInt = (int)result.StatusCode!;
            if (statusCodeInt >= (int)HttpStatusCode.OK && statusCodeInt < (int)HttpStatusCode.BadRequest)
            {
                response.Data = result.Value!;
            }
            else
            {
                var validationProblemDetails = result.Value as ValidationProblemDetails;
                if (validationProblemDetails != null)
                {
                    response.Error = validationProblemDetails.Errors;
                }
                else if (result.Value != null)
                {
                    response.Error = result.Value;
                }

            }
            response.StatusCode = (HttpStatusCode)result.StatusCode;
            result.Value = response;
            return base.ExecuteAsync(context, result);
        }
    }
}
