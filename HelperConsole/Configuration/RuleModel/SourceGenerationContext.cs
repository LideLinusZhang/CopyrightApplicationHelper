using System.Text.Json.Serialization;

namespace HelperConsole.Configuration.RuleModel
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(LanguageRule))]
    [JsonSerializable(typeof(BlockCommentRule))]
    internal partial class SourceGenerationContext : JsonSerializerContext
    {
    }
}
