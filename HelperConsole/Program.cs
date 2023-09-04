using HelperConsole.ConfigurationModel;
using HelperConsole.RuleModel;
using System.Collections.Concurrent;
using System.CommandLine;
using System.Text.Json;

namespace HelperConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var rootCommand = new RootCommand();

            var sourceCodeDirectoryOption = new Option<string>("--source-dir")
            {
                Description = "The path to the source code directory.",
                IsRequired = true 
            };
            var configurationFilePathOption = new Option<string>("--config")
            {
                Description = "The path to the configuration file.",
                IsRequired = true
            };
            var outputFilePathOption = new Option<string>("--output")
            {
                Description = "The path to the output docx file.",
                IsRequired = true
            };
            var projectNameOption = new Option<string>("--project-name")
            {
                Description = "Project name.",
                IsRequired = true
            };
            var projectVersionOption = new Option<string>("--project-ver")
            {
                Description = "Project version.",
                IsRequired = true, 
            };

            sourceCodeDirectoryOption.AddValidator(result =>
            {
                string directory = result.GetValueForOption(sourceCodeDirectoryOption);

                if (!Directory.Exists(directory))
                    result.ErrorMessage = $"The directory {directory} does not exist or is not accessible.";
            });
            configurationFilePathOption.AddValidator(result =>
            {
                string filePath = result.GetValueForOption(configurationFilePathOption);

                if (!File.Exists(filePath))
                    result.ErrorMessage = $"The file {filePath} does not exist or is not accessible.";
            });
            outputFilePathOption.AddValidator(result =>
            {
                string filePath = result.GetValueForOption(configurationFilePathOption);

                if (File.Exists(filePath))
                    result.ErrorMessage = $"The file {filePath} already exists.";
            });

            rootCommand.AddOption(sourceCodeDirectoryOption);
            rootCommand.AddOption(configurationFilePathOption);
            rootCommand.AddOption(outputFilePathOption);
            rootCommand.AddOption(projectNameOption);
            rootCommand.AddOption(projectVersionOption);

            rootCommand.SetHandler(async (context) =>
            {
                string sourceCodeDirectory = context.ParseResult.GetValueForOption(sourceCodeDirectoryOption);
                string configurationFilePath = context.ParseResult.GetValueForOption(configurationFilePathOption);
                string outputFilePath = context.ParseResult.GetValueForOption(outputFilePathOption);
                string projectName = context.ParseResult.GetValueForOption(projectNameOption);
                string projectVersion = context.ParseResult.GetValueForOption(projectVersionOption);

                await Process(sourceCodeDirectory, configurationFilePath, outputFilePath, projectName, projectVersion);
            });

            await rootCommand.InvokeAsync(args);
        }

        private static async Task<List<LanguageRule>> ReadRulesFromConfigurationFile(string configurationFilePath)
        {
            using (var fileStream = new FileStream(configurationFilePath, FileMode.Open, FileAccess.Read))
            {
                return await JsonSerializer.DeserializeAsync(fileStream, SourceGenerationContext.Default.ListLanguageRule);
            }
        }

        private static List<CommentRemover> CreateCommentRemoversBasedOnRules(List<LanguageRule> rules)
        {
            var removers = new List<CommentRemover>();

            foreach (var rule in rules)
                removers.Add(new CommentRemover(rule));

            return removers;
        }

        private static OutputDocument CreateOutputDocument(string title)
        {
            var outputDocument = new OutputDocument();

            outputDocument.AddTitlePage(title);
            outputDocument.AddHeader(title);
            outputDocument.AddPageNumbers();
            outputDocument.AddLineNumbers();

            return outputDocument;
        }

        private static async Task Process(string sourceCodeDirectory, string configurationFilePath, string outputFilePath, string projectName, string projectVersion)
        {
            List<LanguageRule> rules = await ReadRulesFromConfigurationFile(configurationFilePath);

            var removers = new ConcurrentBag<CommentRemover>(CreateCommentRemoversBasedOnRules(rules));

            var processedFileContents = new ConcurrentBag<string>();
            Parallel.ForEach(Directory.EnumerateFiles(sourceCodeDirectory, "*", SearchOption.AllDirectories), async filePath =>
            {
                string extensionName = Path.GetExtension(filePath);

                CommentRemover remover = removers.FirstOrDefault(x => x.IsCompatible(extensionName));

                if (remover is not null)
                    processedFileContents.Add(await remover.ReadWithoutCommentsAsync(filePath));
            });

            using (var outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
            using (OutputDocument outputDocument = CreateOutputDocument(""))
            {

                foreach (var fileContent in processedFileContents)
                {
                    var reader = new StringReader(fileContent);

                    while (await reader.ReadLineAsync() is string line && !string.IsNullOrWhiteSpace(line))
                        outputDocument.AppendLine(line);
                }

                outputDocument.Save(outputFileStream);
            }
        }
    }
}