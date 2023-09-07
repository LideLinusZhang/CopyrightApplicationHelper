namespace HelperConsole.Configuration.RuleModel
{
    internal sealed record BlockCommentRule
    {
        public string StartSymbol { get; set; }
        public string EndSymbol { get; set; }
    }
}
