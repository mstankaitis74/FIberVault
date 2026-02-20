using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FiberVault.Api.Swagger;

public sealed class NodesPostExamplesFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.RelativePath != "api/Nodes" ||
            context.ApiDescription.HttpMethod != "POST")
            return;

        if (operation.RequestBody?.Content?.ContainsKey("application/json") != true)
            return;

        var content = operation.RequestBody.Content["application/json"];
        content.Examples = new Dictionary<string, OpenApiExample>
        {
            ["A"] = new OpenApiExample
            {
                Summary = "A",
                Value = new OpenApiObject
                {
                    ["name"] = new OpenApiString("A"),
                    ["latitude"] = new OpenApiDouble(54.6872),
                    ["longitude"] = new OpenApiDouble(25.2797)
                }
            },
            ["B"] = new OpenApiExample
            {
                Summary = "B",
                Value = new OpenApiObject
                {
                    ["name"] = new OpenApiString("B"),
                    ["latitude"] = new OpenApiDouble(54.6860),
                    ["longitude"] = new OpenApiDouble(25.2850)
                }
            }
        };
    }
}
