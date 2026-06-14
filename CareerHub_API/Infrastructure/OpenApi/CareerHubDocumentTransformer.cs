using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
namespace CareerHub_API.Infrastructure.OpenApi;
public class CareerHubDocumentTransformer: IOpenApiDocumentTransformer
{
     public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
     {
        document.Info.Title = "CareerHub API";
        document.Info.Description = "CareerHub API for managing job listings, companies, applicants, and applications.";
        document.Info.Version = "v1.0.0";
        document.Info.Contact = new OpenApiContact
        {
            Name = "CareerHub Support",
            Email = "support@CareerHub.com",
            Url = new Uri("https://CareerHub.com/support")
        };
        return Task.CompletedTask;
     }
}