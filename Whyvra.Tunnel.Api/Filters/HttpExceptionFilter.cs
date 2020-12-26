using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Whyvra.Tunnel.Common.Models;

namespace Whyvra.Tunnel.Api.Filters
{
    public class HttpExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception.GetType() == typeof(NullReferenceException))
            {
                var error = new ApiMessage {Message = context.Exception.Message, Status = "Not Found", StatusCode = 404};
                context.Result = new JsonResult(error)
                {
                    StatusCode = 404
                };
            }
            else if (context.Exception.GetType() == typeof(ArgumentException))
            {
                var error = new ApiMessage {Message = context.Exception.Message, Status = "Bad Request", StatusCode = 400};
                context.Result = new JsonResult(error)
                {
                    StatusCode = 400
                };
            }
            else
            {
                var error = new ApiMessage {Message = context.Exception.Message, Status = "Inner Server Error", StatusCode = 500};
                if (context.Exception.InnerException != null)
                {
                    error.InnerException = context.Exception.InnerException.Message;
                }

                context.Result = new JsonResult(error)
                {
                    StatusCode = 500
                };
            }
        }
    }
}