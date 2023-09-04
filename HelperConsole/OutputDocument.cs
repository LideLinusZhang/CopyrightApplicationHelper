using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.XWPF.Model;
using NPOI.XWPF.UserModel;

namespace HelperConsole
{
    internal class OutputDocument : IDisposable
    {
        private readonly XWPFDocument _document;

        public OutputDocument() 
        {
            _document = new XWPFDocument();
        }

        public void AddHeader(string content)
        {
            XWPFHeaderFooterPolicy headerFooterPolicy = GetHeaderFooterPolicy();

            XWPFHeader header = headerFooterPolicy.CreateHeader(XWPFHeaderFooterPolicy.DEFAULT);

            XWPFParagraph paragraph = header.CreateParagraph();
            paragraph.Alignment = ParagraphAlignment.RIGHT;

            XWPFRun run = paragraph.CreateRun();
            run.SetText(content);
        }

        public void AddLineNumbers()
        {
            CT_SectPr sectionProperty = _document.Document.body.sectPr;
            sectionProperty.lnNumType = new CT_LineNumber() { countBy = "1", restart = ST_LineNumberRestart.continuous };
        }

        public void AddPageNumbers()
        {
            XWPFHeaderFooterPolicy headerFooterPolicy = GetHeaderFooterPolicy();

            XWPFFooter footer = headerFooterPolicy.CreateFooter(XWPFHeaderFooterPolicy.DEFAULT);

            XWPFParagraph paragraph = footer.CreateParagraph();
            paragraph.Alignment = ParagraphAlignment.CENTER;
            paragraph.GetCTP().AddNewFldSimple().instr = "PAGE \\* MERGEFORMAT";
        }

        public void AddTitlePage(string title)
        {
            XWPFParagraph coverPageParagraph = _document.CreateParagraph();
            coverPageParagraph.Alignment = ParagraphAlignment.CENTER;
            coverPageParagraph.VerticalAlignment = TextAlignment.CENTER;
            coverPageParagraph.GetCTP().AddNewPPr().suppressLineNumbers = new CT_OnOff() { val = true };

            XWPFRun contentRun = coverPageParagraph.CreateRun();
            contentRun.SetText(title);

            XWPFRun pageBreakRun = coverPageParagraph.CreateRun();
            pageBreakRun.AddBreak(BreakType.PAGE);

            CT_SectPr sectionProperty = _document.Document.body.sectPr;
            sectionProperty.titlePg = new CT_OnOff() { val = true };

            // Set the header and footer on the title page both to empty.
            XWPFHeaderFooterPolicy headerFooterPolicy = GetHeaderFooterPolicy();
            headerFooterPolicy.CreateFooter(XWPFHeaderFooterPolicy.FIRST);
            headerFooterPolicy.CreateHeader(XWPFHeaderFooterPolicy.FIRST);
        }

        public void AppendLine(string line)
        {
            _document.CreateParagraph().CreateRun().SetText(line);
        }

        public void Dispose() => _document.Dispose();

        public void Save(FileStream stream) => _document.Write(stream);

        private XWPFHeaderFooterPolicy GetHeaderFooterPolicy() 
        {
            return _document.GetHeaderFooterPolicy() ?? _document.CreateHeaderFooterPolicy();
        }
    }
}
