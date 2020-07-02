using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace CopyrightApplicationHelper
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormCopyrightApplicationHelper());
        }
    }

    class Rule
    {
        public string FileExtension { get; set; } = string.Empty;
        public string LineComment { get; set; } = string.Empty;
        public string BlockCommentStart { get; set; } = string.Empty;
        public string BlockCommentEnd { get; set; } = string.Empty;
        public Regex LineCommentRegex = null;
        public Regex BlockCommentRegex = null;

        public void GenerateRegex()
        {
            if (LineComment != string.Empty)
                LineCommentRegex = new Regex(@"[ \t\v]*" + LineComment + @".*", RegexOptions.Compiled);
            if (BlockCommentStart != string.Empty && BlockCommentEnd != string.Empty)
                BlockCommentRegex = new Regex(BlockCommentStart + @".*" + BlockCommentEnd,
                    RegexOptions.Compiled | RegexOptions.Singleline);
        }
    }

    static class RuleImporter
    {
        private static readonly string FileBlockStart = "File";
        private static readonly string BlockEnd = "End";
        private static readonly string LineCommentLineStart = "LineComment";
        private static readonly string BlockCommentStartLineStart = "BlockCommentStart";
        private static readonly string BlockCommentEndLineStart = "BlockCommentEnd";

        public static void ImportRules(string ruleFilePath)
        {
            FileStream ruleFile = new FileStream(ruleFilePath, FileMode.Open, FileAccess.Read);
            StreamReader ruleReader = new StreamReader(ruleFile);
            bool isInFileBlock = false;
            Rule rule = new Rule();
            do
            {
                string currentLine = ruleReader.ReadLine();

                if (currentLine.StartsWith(FileBlockStart))
                {
                    rule = new Rule();
                    isInFileBlock = true;
                    rule.FileExtension = ExtractValue(currentLine);
                }
                else if (isInFileBlock)
                {
                    if (currentLine.StartsWith(LineCommentLineStart))
                    {
                        rule.LineComment = Regex.Escape(ExtractValue(currentLine));
                    }
                    else if (currentLine.StartsWith(BlockCommentStartLineStart))
                    {
                        rule.BlockCommentStart = Regex.Escape(ExtractValue(currentLine));
                    }
                    else if (currentLine.StartsWith(BlockCommentEndLineStart))
                    {
                        rule.BlockCommentEnd = Regex.Escape(ExtractValue(currentLine));
                    }
                    else if (currentLine.StartsWith(BlockEnd))
                    {
                        isInFileBlock = false;
                        rule.GenerateRegex();
                        Parser.AddRule(rule);
                        ProgressInfo.ShowProgress(rule.FileExtension, ProgressInfo.Stage.RuleImporting);
                    }
                }
            } while (!ruleReader.EndOfStream);
        }

        private static string ExtractValue(string line)
        {
            int index = -1, length = 0;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '\"')
                {
                    if (index == -1)
                        index = i + 1;
                    else
                    {
                        length = i - index;
                        break;
                    }
                }
            }

            return line.Substring(index, length);
        }
    }

    static class Parser
    {
        private static List<Rule> Rules = new List<Rule>();
        public static StringBuilder ParsedLines { get; set; } = new StringBuilder();
        private static readonly object _ParsedLinesSync = new object();

        public static void TraverseSourceForParse(string sourceDirectory)
        {
            Parallel.ForEach(Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories), filePath => ParseFile(filePath, FindRule(filePath)));
        }

        public static void AddRule(Rule rule)
        {
            Rules.Add(rule);
        }
        private static void ParseFile(string filePath, Rule rule)
        {
            if (rule is null)
                return;

            StreamReader reader = new StreamReader(filePath);
            string fileContent = reader.ReadToEnd().Normalize();

            if (!(rule.LineCommentRegex is null))
                fileContent = rule.LineCommentRegex.Replace(fileContent, string.Empty);
            if (!(rule.BlockCommentRegex is null))
                fileContent = rule.BlockCommentRegex.Replace(fileContent, string.Empty);

            lock (_ParsedLinesSync)
            {
                ParsedLines.Append(fileContent.RemoveEmptyLines());
                ProgressInfo.ShowProgress(filePath, ProgressInfo.Stage.FileParsed);
            }
        }

        private static string RemoveEmptyLines(this string content)
        {
            StringBuilder removed = new StringBuilder();

            foreach (string line in content.Split('\n', '\r'))
                if (line.Trim() != string.Empty)
                    removed.AppendLine(line);

            return removed.ToString();
        }

        private static Rule FindRule(string path)
        {
            foreach (Rule rule in Rules)
                if (path.EndsWith(rule.FileExtension))
                    return rule;

            return null;
        }
    }

    static class Output
    {
        private static readonly double StandardFontSize = 10.0;
        private static readonly double FirstPageFontSize = 72.0;
        public static string NameAndVersion { get; set; } = string.Empty;

        public static void OutputDocx(FileStream docxFile)
        {
            ProgressInfo.ShowProgress("开始将处理结果输出至生成文件。", ProgressInfo.Stage.DocxOutputStarted);

            using (DocX outputDocx = DocX.Create(docxFile))
            {
                outputDocx.AddHeaders();
                outputDocx.DifferentFirstPage = true;

                outputDocx.Headers.Odd.InsertParagraph(NameAndVersion + "源代码");
                outputDocx.Sections[0].PageNumberStart = 0;
                outputDocx.Headers.Odd.PageNumbers = true;
                outputDocx.Headers.Odd.PageNumberParagraph.Alignment = Alignment.right;

                Paragraph firstPage = outputDocx.InsertParagraph("\n" + NameAndVersion + "源代码");
                firstPage.FontSize(FirstPageFontSize);
                firstPage.Alignment = Alignment.center;
                firstPage.InsertPageBreakAfterSelf();

                StringReader codeLines = new StringReader(Parser.ParsedLines.ToString());

                do
                {
                    Paragraph codeLine = outputDocx.InsertParagraph(codeLines.ReadLine());
                    codeLine.FontSize(StandardFontSize);
                } while (codeLines.Peek() != -1);

                outputDocx.Save();
            }

            ProgressInfo.ShowProgress("已将处理结果输出至生成文件。", ProgressInfo.Stage.DocxOutputFinished);
        }
    }

    static class ProgressInfo
    {
        public static Action<string> AppendListBoxProgress;
        public enum Stage
        {
            RuleImporting,
            FileParsed,
            DocxOutputStarted,
            DocxOutputFinished,
            Other
        };

        public static void ShowProgress(string info, Stage stage)
        {
            string progressInfo;

            switch (stage)
            {
                case Stage.RuleImporting:
                    progressInfo = string.Format("{0} 文件处理规则已导入", info);
                    break;
                case Stage.FileParsed:
                    progressInfo = string.Format("已处理 {0}", info);
                    break;
                default:
                    progressInfo = info;
                    break;
            }

            AppendListBoxProgress(progressInfo);
        }
    }
}
