using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Whyvra.Tunnel.Api.Filters
{
    public class HttpExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(NullReferenceException))
            {
                var error = new {message = context.Exception.Message, status = "Not Found", statusCode = 404};
                context.Result = new JsonResult(error)
                {
                    StatusCode = 404
                };
            }

            if (context.Exception.GetType() == typeof(ArgumentException))
            {
                var error = new {message = context.Exception.Message, status = "Bad Request", statusCode = 400};
                context.Result = new JsonResult(error)
                {
                    StatusCode = 400
                };
            }
        }
    }
}