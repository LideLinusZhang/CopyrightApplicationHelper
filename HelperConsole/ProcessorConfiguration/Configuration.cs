using HelperConsole.Configuration.RuleModel;
using System.Text.Json;

namespace HelperConsole.Configuration
{
    internal static class ConfigurationReader
    {
        public static IAsyncEnumerable<LanguageRule> ReadFromStream(Stream stream)
        {
            return JsonSerializer.DeserializeAsyncEnumerable(stream, SourceGenerationContext.Default.LanguageRule);
        }
    }
}
