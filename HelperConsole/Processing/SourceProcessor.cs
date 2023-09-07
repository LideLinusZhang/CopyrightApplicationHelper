using HelperConsole.Configuration.RuleModel;
using HelperConsole.Extensions;

namespace HelperConsole.Processing
{
    internal sealed class SourceProcessor
    {
        private readonly List<CommentRemover> _removers;

        public delegate void FileProcessedAction(string filePath);

        public event FileProcessedAction OnFileProcessed;

        private SourceProcessor(List<CommentRemover> removers)
        {
            _removers = removers;
        }

        public async IAsyncEnumerable<string> GetProcessedSourceLinesAsync(string sourceCodeDirectory)
        {
            foreach (var filePath in Directory.EnumerateFiles(sourceCodeDirectory, "*", SearchOption.AllDirectories))
            {
                string extensionName = Path.GetExtension(filePath);

                CommentRemover remover = _removers.Find(x => x.IsCompatible(extensionName));

                if (remover is null)
                    continue;

                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (var fileReader = new StreamReader(fileStream))
                {
                    IAsyncEnumerable<string> rawLines = fileReader.ReadAllLinesAsync();

                    await foreach (var lineWithoutComment in remover.RemoveCommentsAsync(rawLines))
                    {
                        if (!string.IsNullOrEmpty(lineWithoutComment))
                            yield return lineWithoutComment;
                    }
                }

                OnFileProcessed(filePath);
            }
        }

        public static async Task<SourceProcessor> CreateFromRulesAsync(IAsyncEnumerable<LanguageRule> rules)
        {
            var removers = new List<CommentRemover>();

            await foreach (var rule in rules)
                removers.Add(new CommentRemover(rule));

            return new SourceProcessor(removers);
        }
    }
}
