using HelperConsole.Configuration.RuleModel;
using HelperConsole.Extensions;

namespace HelperConsole.Processing
{
    internal sealed class CommentRemover
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

            _blockCommentBeginSymbols = _blockCommentRules.Select(x => x.StartSymbol).ToList();
        }

        public async IAsyncEnumerable<string> RemoveCommentsAsync(IAsyncEnumerable<string> lines)
        {
            bool isInBlockComment = false;
            BlockCommentRule blockCommentRule = null;

            await foreach (var rawLine in lines)
            {
                if (isInBlockComment)
                {
                    int blockCommendEndSymbolIndex = rawLine.IndexOf(blockCommentRule.EndSymbol);

                    if (blockCommendEndSymbolIndex != -1)
                    {
                        string removed = rawLine.Substring(blockCommendEndSymbolIndex + 1);

                        isInBlockComment = false;
                        yield return removed;
                    }
                }
                else
                {
                    int lineCommendSymbolIndex = rawLine.IndexOfAny(_lineCommentSymbols);

                    if (lineCommendSymbolIndex != -1)
                    {
                        string removed = rawLine.Substring(0, lineCommendSymbolIndex);

                        yield return removed;
                        continue;
                    }

                    string blockCommentBeginSymbol;
                    int blockCommendBeginSymbolIndex = rawLine.IndexOfAny(_blockCommentBeginSymbols, out blockCommentBeginSymbol);

                    if (blockCommendBeginSymbolIndex != -1)
                    {
                        isInBlockComment = true;
                        blockCommentRule = _blockCommentRules.First(rule => rule.StartSymbol == blockCommentBeginSymbol);

                        string removed = rawLine.Substring(0, blockCommendBeginSymbolIndex);

                        yield return removed;
                        continue;
                    }

                    yield return rawLine;
                }
            }
        }

        public bool IsCompatible(string extensionName) => _extensionNames.Contains(extensionName);
    }
}
