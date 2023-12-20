using System.Text.Json.Serialization;

namespace CopyrightHelper.Library.Configuration.RuleModel
{
    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(LanguageRule))]
    [JsonSerializable(typeof(BlockCommentRule))]
    public partial class SourceGenerationContext : JsonSerializerContext
    {
    }
}
