namespace HelperConsole.Configuration.RuleModel
{
    internal sealed record LanguageRule
    {
        public string Name { get; set; }
        public List<string> ExtensionNames { get; set; }
        public List<BlockCommentRule> BlockCommentRules { get; set; }
        public List<string> LineCommentSymbols { get; set; }
    }
}
