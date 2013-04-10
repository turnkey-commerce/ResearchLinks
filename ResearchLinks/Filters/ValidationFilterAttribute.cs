using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;
using System.Text;

namespace ResearchLinks.Filters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
         public override void OnActionExecuting(HttpActionContext actionContext)
         {
             if (!actionContext.ModelState.IsValid)
             {
                 var errors = new StringBuilder();
                 foreach (var error in actionContext.ModelState.Values.SelectMany(modelState => modelState.Errors))
                 {
                     errors.Append(error.ErrorMessage + " ");
                 }
                 actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errors.ToString());
             }
         }
    }
}