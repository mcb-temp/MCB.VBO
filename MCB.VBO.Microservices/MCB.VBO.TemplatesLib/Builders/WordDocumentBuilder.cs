using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MCB.VBO.Microservices.Statements.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCB.VBO.TemplatesLib.Builders
{
    public class WordDocumentBuilder : IDisposable
    {
        private MemoryStream _memoryStream;
        private WordprocessingDocument _wordDocument;


        public WordDocumentBuilder()
        {
            _memoryStream = new MemoryStream();
            _wordDocument = WordprocessingDocument.Create(_memoryStream, WordprocessingDocumentType.Document, true);
            MainDocumentPart mainPart = _wordDocument.AddMainDocumentPart();
            new Document(new Body()).Save(mainPart);
           
            /*
            // Create Stream
            using (MemoryStream mem = new MemoryStream())
            {
                // Create Document
                using (WordprocessingDocument wordDocument =
                    WordprocessingDocument.Create(mem, WordprocessingDocumentType.Document, true))
                {
                    // Add a main document part. 
                    MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

                    // Create the document structure and add some text.
                    mainPart.Document = new Document();
                    Body docBody = new Body();

                    // Add your docx content here
                }

                // Download File
                //Context.Response.AppendHeader("Content-Disposition", String.Format("attachment;filename=\"0}.docx\"", MyDocxTitle));
                //mem.Position = 0;
                //mem.CopyTo(Context.Response.OutputStream);
                //Context.Response.Flush();
                //Context.Response.End();
            }
            */
        }

        public void BuildHeader(StatementData data)
        {
            Paragraph p = new Paragraph();
            ParagraphProperties pp = new ParagraphProperties();
            pp.Justification = new Justification() { Val = JustificationValues.Center };
            // Add paragraph properties to your paragraph
            p.Append(pp);
            // Run
            Run r = new Run();
            Text t = new Text($"{data.Name}") { Space = SpaceProcessingModeValues.Preserve };
            r.Append(t);
            p.Append(r);
            // Add your paragraph to docx body
            _wordDocument.MainDocumentPart.Document.Body.Append(p);
            _wordDocument.Save();
            _wordDocument.MainDocumentPart.Document.Save();
        }

        public void BuildTable(List<StatementTransaction> statementTransactions)
        {
            Table table = new Table();

            TableRow tr1 = new TableRow();

            TableCell tc11 = new TableCell();
            Paragraph p11 = new Paragraph(new Run(new Text("Дата")));
            tc11.Append(p11);
            tr1.Append(tc11);

            TableCell tc12 = new TableCell();
            Paragraph p12 = new Paragraph();
            Run r12 = new Run();
            RunProperties rp12 = new RunProperties();
            rp12.Bold = new Bold();
            r12.Append(rp12);
            r12.Append(new Text("Сумма"));
            p12.Append(r12);
            tc12.Append(p12);

            tr1.Append(tc12);
            table.Append(tr1);

            TableCell tc13 = new TableCell();
            Paragraph p13 = new Paragraph(new Run(new Text("Отправитель")));
            tc13.Append(p13);
            tr1.Append(tc13);

            TableCell tc14 = new TableCell();
            Paragraph p14 = new Paragraph(new Run(new Text("Получатель")));
            tc13.Append(p14);
            tr1.Append(tc14);

            TableRow tr2 = new TableRow();

            TableCell tc21 = new TableCell();
            Paragraph p21 = new Paragraph(new Run(new Text("Little")));
            tc21.Append(p21);
            tr2.Append(tc21);

            TableCell tc22 = new TableCell();
            Paragraph p22 = new Paragraph();
            ParagraphProperties pp22 = new ParagraphProperties();
            pp22.Justification = new Justification() { Val = JustificationValues.Center };
            p22.Append(pp22);
            p22.Append(new Run(new Text("Table")));
            tc22.Append(p22);

            tr2.Append(tc22);
            table.Append(tr2);

            // Add your table to docx body
            _wordDocument.MainDocumentPart.Document.Body.Append(table);
            _wordDocument.Save();
            _wordDocument.MainDocumentPart.Document.Save();
        }

        public Stream GetResult()
        {
            MemoryStream stream = new MemoryStream(_memoryStream.Capacity);
            _memoryStream.Position = 0;
            _memoryStream.CopyTo(stream);
            stream.Position = 0;

            return stream;
        }

        ~WordDocumentBuilder()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_wordDocument != null)
                _wordDocument.Dispose();
            if (_memoryStream != null)
                _memoryStream.Dispose();
        }
    }
}
