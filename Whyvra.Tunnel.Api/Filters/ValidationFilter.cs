using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Whyvra.Accounts.Api.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ModelState.IsValid) return;

            var errors = filterContext.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage.Replace("'", ""))
                .ToList();

            var error = new {message = errors[0], status = "Bad Request", statusCode = 400};
            filterContext.Result = new JsonResult(error) {StatusCode = 400};
        }
    }
}