using System.Collections.Generic;

namespace CopyrightHelper.Library.Configuration.RuleModel
{
    public sealed record LanguageRule
    {
        public string Name { get; set; }
        public List<string> ExtensionNames { get; set; }
        public List<BlockCommentRule> BlockCommentRules { get; set; }
        public List<string> LineCommentSymbols { get; set; }
    }
}
