﻿using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Server.SignalR.Infrastructure
{
    public class ApplySummariesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if(controllerActionDescriptor == null)
            {
                return;
            }
            var actionName = controllerActionDescriptor.ActionName;
            if(actionName != "GetPaged")
            {
                return;
            }
            var resourceName = controllerActionDescriptor.ControllerName;
            operation.Summary = $"Возвращает постраничный список {resourceName} как IPagedList, обернутый OperationResult";
        }
    }
}