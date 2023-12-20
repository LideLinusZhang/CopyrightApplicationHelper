using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using Header = DocumentFormat.OpenXml.Wordprocessing.Header;
using Run = DocumentFormat.OpenXml.Wordprocessing.Run;
using SectionProperties = DocumentFormat.OpenXml.Wordprocessing.SectionProperties;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace CopyrightHelper.Output
{
    public sealed class DocxDocument : IDisposable
    {
        private readonly WordprocessingDocument _wordProccessingDocument;

        private readonly MainDocumentPart _mainPart;

        private readonly Document _document;
        private readonly Body _body;

        private readonly HeaderPart _headerPart;
        private readonly FooterPart _footerPart;

        private readonly SectionProperties _sectionProperties;

        private DocxDocument(string path)
        {
            _wordProccessingDocument = WordprocessingDocument.Create(path, WordprocessingDocumentType.Document, false);

            _mainPart = _wordProccessingDocument.AddMainDocumentPart();

            _mainPart.Document = new Document();
            _document = _mainPart.Document;

            _body = _document.AppendChild(new Body());

            _headerPart = _mainPart.AddNewPart<HeaderPart>();
            _footerPart = _mainPart.AddNewPart<FooterPart>();

            _sectionProperties = new SectionProperties();
        }

        public void AppendLine(string line)
        {
            var paragraph = _body.AppendChild(new Paragraph());

            paragraph.AppendChild(new Run())
                     .AppendChild(new Text(line) { Space = SpaceProcessingModeValues.Preserve });

            paragraph.ParagraphProperties = new ParagraphProperties()
            {
                Justification = new Justification() { Val = JustificationValues.Left },
            };
        }

        public void Save()
        {
            _body.RemoveAllChildren<SectionProperties>();

            _body.AppendChild(_sectionProperties);

            _wordProccessingDocument.Save();
        }

        public void Dispose() => _wordProccessingDocument.Dispose();

        private void AddHeader(string content)
        {
            var header = new Header();

            var paragraph = header.AppendChild(new Paragraph());

            paragraph.AppendChild(new Run())
                     .AppendChild(new Text(content));

            paragraph.ParagraphProperties = new ParagraphProperties()
            {
                Justification = new Justification() { Val = JustificationValues.Right }
            };

            _headerPart.Header = header;

            var headerReference = new HeaderReference()
            {
                Id = _mainPart.GetIdOfPart(_headerPart),
                Type = HeaderFooterValues.Default
            };

            _sectionProperties.PrependChild(headerReference);
        }

        private void AddLineNumbers()
        {
            var lineNumberType = new LineNumberType()
            {
                Start = 0,
                CountBy = 1,
                Restart = LineNumberRestartValues.Continuous
            };

            _sectionProperties.AppendChild(lineNumberType);
        }

        private void AddPageNumbers()
        {
            var footer = new Footer();

            var paragraph = footer.AppendChild(new Paragraph());

            paragraph.AppendChild(new SimpleField() { Instruction = "PAGE \\* MERGEFORMAT" });

            paragraph.ParagraphProperties = new ParagraphProperties()
            {
                Justification = new Justification() { Val = JustificationValues.Center }
            };

            _footerPart.Footer = footer;

            var footerReference = new FooterReference()
            {
                Id = _mainPart.GetIdOfPart(_footerPart),
                Type = HeaderFooterValues.Default
            };

            _sectionProperties.PrependChild(footerReference);
        }

        private void AddTitlePage(string title, string subTitle)
        {
            Paragraph coverPageParagraph = _body.AppendChild(new Paragraph());

            coverPageParagraph.ParagraphProperties = new ParagraphProperties()
            {
                Justification = new Justification() { Val = JustificationValues.Center },
                SuppressLineNumbers = new SuppressLineNumbers() { Val = true },
                SpacingBetweenLines = new SpacingBetweenLines() { BeforeLines = 1000 }
            };

            Run contentRun = coverPageParagraph.AppendChild(new Run());

            contentRun.AppendChild(new Text(title));
            contentRun.AppendChild(new Break() { Type = BreakValues.TextWrapping });

            contentRun.RunProperties = new RunProperties()
            {
                FontSize = new FontSize() { Val = "36" }
            };

            contentRun = coverPageParagraph.AppendChild(new Run());

            contentRun.AppendChild(new Text(subTitle));
            contentRun.AppendChild(new Break() { Type = BreakValues.Page });

            contentRun.RunProperties = new RunProperties()
            {
                FontSize = new FontSize() { Val = "48" }
            };

            _sectionProperties.AppendChild(new TitlePage() { Val = true });

            // Set the header and footer on the title page both to empty.
            var emptyTitlePageHeaderPart = _mainPart.AddNewPart<HeaderPart>();
            var emptyTitlePageFooterPart = _mainPart.AddNewPart<FooterPart>();

            emptyTitlePageHeaderPart.Header = new Header();
            emptyTitlePageFooterPart.Footer = new Footer();

            var headerReference = new HeaderReference()
            {
                Id = _mainPart.GetIdOfPart(emptyTitlePageHeaderPart),
                Type = HeaderFooterValues.Default
            };
            var footerReference = new FooterReference()
            {
                Id = _mainPart.GetIdOfPart(emptyTitlePageFooterPart),
                Type = HeaderFooterValues.Default
            };

            _sectionProperties.PrependChild(headerReference);
            _sectionProperties.PrependChild(footerReference);
        }

        public static DocxDocument CreateDocument(string title, string subTitle, string path)
        {
            var outputDocument = new DocxDocument(path);

            outputDocument.AddTitlePage(title, subTitle);
            outputDocument.AddHeader($"{title} {subTitle}");
            outputDocument.AddPageNumbers();
            outputDocument.AddLineNumbers();

            return outputDocument;
        }
    }
}
