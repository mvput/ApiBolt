using System.Text;

namespace ApiBolt.SourceGenerator;

public static class SourceGenerationHelper
{
    public static string GenerateRegistrationClass(ApiEndpointToGenerate value)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"namespace {value.Ns};");
        sb.AppendLine();
        sb.Append("internal class ").Append(value.Name).Append("Registration : global::ApiBolt.Abstractions.IApiEndpointRegistration").AppendLine();
        sb.AppendLine("{");
        sb.AppendLine("public void MapEndpoint(IEndpointRouteBuilder app)");
        sb.AppendLine("{");

        if (value.EndpointType == ApiEndpointType.Get)
        {
            sb.Append("app.MapGet(\"").Append(value.Pattern).Append($"\", (   [global::Microsoft.AspNetCore.Mvc.FromServicesAttribute]  {value.Name} api) => api.GetRiskProfile());").AppendLine();
        }
        
        sb.AppendLine("}");
        sb.AppendLine("}");


        return sb.ToString();
    }
}