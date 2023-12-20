namespace CopyrightHelper.Library.Configuration.RuleModel
{
    public sealed record BlockCommentRule
    {
        public string StartSymbol { get; set; }
        public string EndSymbol { get; set; }
    }
}
