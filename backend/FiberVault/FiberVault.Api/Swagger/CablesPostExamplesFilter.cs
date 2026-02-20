using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FiberVault.Api.Swagger;

public sealed class CablesPostExamplesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.RelativePath != "api/Cables" ||
            context.ApiDescription.HttpMethod != "POST")
            return;

        if (operation.RequestBody?.Content?.ContainsKey("application/json") != true)
            return;

        var content = operation.RequestBody.Content["application/json"];
        content.Examples = new Dictionary<string, OpenApiExample>
        {
            ["Cable-001"] = new OpenApiExample
            {
                Summary = "Cable-001",
                Value = new OpenApiObject
                {
                    ["name"] = new OpenApiString("Cable-001"),
                    ["fromNodeId"] = new OpenApiString("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    ["toNodeId"] = new OpenApiString("11111111-2222-3333-4444-555555555555"),
                    ["path"] = new OpenApiArray
                    {
                        new OpenApiObject
                        {
                            ["longitude"] = new OpenApiDouble(25.2797),
                            ["latitude"] = new OpenApiDouble(54.6872)
                        },
                        new OpenApiObject
                        {
                            ["longitude"] = new OpenApiDouble(25.2810),
                            ["latitude"] = new OpenApiDouble(54.6870)
                        },
                        new OpenApiObject
                        {
                            ["longitude"] = new OpenApiDouble(25.2830),
                            ["latitude"] = new OpenApiDouble(54.6865)
                        },
                        new OpenApiObject
                        {
                            ["longitude"] = new OpenApiDouble(25.2850),
                            ["latitude"] = new OpenApiDouble(54.6860)
                        }
                    }
                }
            }
        };
    }
}
