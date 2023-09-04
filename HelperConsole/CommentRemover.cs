using HelperConsole.ConfigurationModel;
using HelperConsole.Extensions;
using HelperConsole.RuleModel;
using System.Text;

namespace HelperConsole
{
    internal class CommentRemover
    {
        private readonly List<BlockCommentRule> _blockCommentRules;
        private readonly List<string> _blockCommentBeginSymbols;
        private readonly List<string> _extensionNames;
        private readonly List<string> _lineCommentSymbols;

        public CommentRemover(LanguageRule rule)
        {
            _blockCommentRules = rule.BlockCommentRules;
            _extensionNames = rule.ExtensionNames;
            _lineCommentSymbols = rule.LineCommentSymbols;

            _blockCommentBeginSymbols = _blockCommentRules.Select(rule => rule.StartSymbol).ToList();
        }

        public async Task<string> ReadWithoutCommentsAsync(string filePath)
        {
            var contentStringBuilder = new StringBuilder();

            using (StreamReader reader = new StreamReader(filePath))
            {
                bool isInBlockComment = false;
                BlockCommentRule blockCommentRule = null;

                while (!reader.EndOfStream)
                {
                    string raw = await reader.ReadLineAsync();

                    if (isInBlockComment)
                    {
                        int blockCommendEndSymbolIndex = raw.IndexOf(blockCommentRule.EndSymbol);

                        if (blockCommendEndSymbolIndex != -1)
                        {
                            string removed = raw.Substring(blockCommendEndSymbolIndex + 1);

                            contentStringBuilder.AppendLine(removed);

                            isInBlockComment = false;
                        }
                    }
                    else
                    {
                        int lineCommendSymbolIndex = raw.IndexOfAny(_lineCommentSymbols);

                        if (lineCommendSymbolIndex != -1)
                        {
                            string removed = raw.Substring(0, lineCommendSymbolIndex);

                            contentStringBuilder.AppendLine(removed);
                            continue;
                        }

                        string blockCommentBeginSymbol;
                        int blockCommendBeginSymbolIndex = raw.IndexOfAny(_blockCommentBeginSymbols, out blockCommentBeginSymbol);

                        if (blockCommendBeginSymbolIndex != -1)
                        {
                            isInBlockComment = true;
                            blockCommentRule = _blockCommentRules.First(rule => rule.StartSymbol == blockCommentBeginSymbol);

                            string removed = raw.Substring(0, blockCommendBeginSymbolIndex);

                            contentStringBuilder.AppendLine(removed);
                            continue;
                        }

                        contentStringBuilder.AppendLine(raw);
                    }
                }
            }

            return contentStringBuilder.ToString();
        }

        public bool IsCompatible(string extensionName) => _extensionNames.Contains(extensionName);
    }
}
