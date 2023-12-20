using CopyrightHelper.Library.Configuration;
using CopyrightHelper.Library.Processing;
using CopyrightHelper.Output;
using System.CommandLine;

namespace HelperConsole
{
    internal class Program
    {
        public static async Task Main(string[] args)
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
                string filePath = result.GetValueForOption(outputFilePathOption);

                if (File.Exists(filePath))
                    result.ErrorMessage = $"The file {filePath} already exists or cannot be accessed.";
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

                string title = $"{projectName} {projectVersion}";

                var configurationReader = new JsonConfigurationReader();

                using (var configurationStream = new FileStream(configurationFilePath, FileMode.Open, FileAccess.Read))
                using (var outputDocument = DocxDocument.CreateDocument(title, "源代码", outputFilePath))
                {
                    var rules = configurationReader.ReadFromStream(configurationStream);

                    var processor = await SourceProcessor.CreateFromRulesAsync(rules);
                    processor.OnFileProcessed += (filePath) => Console.WriteLine($"File processed: {filePath}");

                    await foreach (var lines in processor.GetProcessedSourceLinesAsync(sourceCodeDirectory))
                        outputDocument.AppendLine(lines);

                    outputDocument.Save();
                }
            });

            await rootCommand.InvokeAsync(args);
        }
    }
}