using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using WebUI.Controllers;

namespace WebUI.Filters
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor == null || !descriptor.ActionName.Contains(nameof(CategoriesController.UpdateImage))) return;
            operation.Parameters.Clear();

            var uploadFileMediaType = new OpenApiMediaType {
                Schema = new OpenApiSchema {
                    Type = "object",
                    Properties =
                    {
                        ["uploadedFile"] = new OpenApiSchema
                        {
                            Description = "Upload File",
                            Type = "file",
                            Format = "binary"
                        }
                    },
                    Required = new HashSet<string>
                    {
                        "uploadedFile"
                    }
                }
            };

            operation.RequestBody = new OpenApiRequestBody {
                Content =
                {
                    ["multipart/form-data"] = uploadFileMediaType
                }
            };

            operation.Parameters.Add(new OpenApiParameter() {
                Name = "id",
                In = ParameterLocation.Path,
                Description = "Id of category",
                Required = true
            });
        }
    }
}
