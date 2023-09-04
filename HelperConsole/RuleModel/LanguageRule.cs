using HelperConsole.RuleModel;

namespace HelperConsole.ConfigurationModel
{
    internal sealed record LanguageRule
    {
        public string Name { get; }
        public List<string> ExtensionNames { get; }
        public List<BlockCommentRule> BlockCommentRules { get; }
        public List<string> LineCommentSymbols { get; }
    }
}
