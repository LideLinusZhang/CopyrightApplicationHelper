using HelperConsole.Configuration.RuleModel;
using System.Text.Json;

namespace HelperConsole.Configuration
{
    internal sealed class JsonConfigurationReader : IConfigurationReader
    {
        public IAsyncEnumerable<LanguageRule> ReadFromStream(Stream stream)
        {
            return JsonSerializer.DeserializeAsyncEnumerable(stream, SourceGenerationContext.Default.LanguageRule);
        }
    }
}
