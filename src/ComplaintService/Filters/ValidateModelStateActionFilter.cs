using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComplaintService.BusinessDomain.ApplicationModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ComplaintService.Filters
{
    public class ValidateModelStateActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Check if ModelState is valid.
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Keys.Where(i => context.ModelState[i].Errors.Count > 0)
                    .Select(k => new ValidationErrorModel
                    {
                        Field = k,
                        Message = context.ModelState[k].Errors.First().ErrorMessage
                    }).ToList();

                context.Result = new BadRequestObjectResult(new ApiError<ValidationErrorModel>(StatusCodes.Status400BadRequest, "Your request parameters didn't validate", "Model input is not correct", errors));
            }
            else
            {
                await next();
            }
        }
    }
}