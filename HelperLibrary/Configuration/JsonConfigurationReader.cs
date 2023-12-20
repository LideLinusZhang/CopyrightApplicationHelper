using CopyrightHelper.Library.Configuration.RuleModel;
using System.IO;
using System.Text.Json;

namespace CopyrightHelper.Library.Configuration
{
    public sealed class JsonConfigurationReader : IConfigurationReader
    {
        public System.Collections.Generic.IAsyncEnumerable<LanguageRule> ReadFromStream(Stream stream)
        {
            return JsonSerializer.DeserializeAsyncEnumerable(stream, SourceGenerationContext.Default.LanguageRule);
        }
    }
}
