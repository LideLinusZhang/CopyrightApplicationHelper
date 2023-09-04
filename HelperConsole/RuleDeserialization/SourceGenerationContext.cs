using HelperConsole.ConfigurationModel;
using System.Text.Json.Serialization;

namespace HelperConsole.RuleModel
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(List<LanguageRule>))]
    [JsonSerializable(typeof(LanguageRule))]
    [JsonSerializable(typeof(BlockCommentRule))]
    internal partial class SourceGenerationContext : JsonSerializerContext
    {
    }
}
