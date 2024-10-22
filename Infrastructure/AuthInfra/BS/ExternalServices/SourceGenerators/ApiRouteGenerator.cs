using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace BS.ExternalServices.SourceGenerators
{
    [Generator]
    public class ApiRouteGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Register a syntax receiver that will be created for every compilation
            var featureInterfaces = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (s, _) => IsFeatureInterface(s),
                    transform: static (ctx, _) => GetInterfaceName(ctx))
                .Where(static name => name is not null);

            context.RegisterSourceOutput(featureInterfaces, (spc, interfaceName) =>
            {
                if (interfaceName is not null)
                {
                    var source = GenerateApiRoutes(interfaceName);
                    spc.AddSource($"{interfaceName}_ApiRoutes.g.cs", SourceText.From(source, Encoding.UTF8));
                }
            });
        }

        private static bool IsFeatureInterface(SyntaxNode syntaxNode)
        {
            // This is a simple example, you can expand the logic as needed
            return syntaxNode is InterfaceDeclarationSyntax { Identifier.Text: "IFeature" };
        }

        private static string? GetInterfaceName(GeneratorSyntaxContext context)
        {
            var interfaceDeclaration = (InterfaceDeclarationSyntax)context.Node;
            return interfaceDeclaration.Identifier.Text;
        }

        private static string GenerateApiRoutes(string interfaceName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("namespace GeneratedApiRoutes");
            sb.AppendLine("{");
            sb.AppendLine($"    public static class {interfaceName}ApiRoutes");
            sb.AppendLine("    {");
            sb.AppendLine("        public static readonly string[] Routes = new[]");
            sb.AppendLine("        {");
            // Add your route generation logic here
            sb.AppendLine("            \"/GetOverallActions\",");
            // Add other routes if needed
            sb.AppendLine("        };");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}