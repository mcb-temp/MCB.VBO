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

        private MainDocumentPart MainDocumentPart => _wordDocument.MainDocumentPart;
        private Document Document => _wordDocument.MainDocumentPart.Document;
        private Body Body => _wordDocument.MainDocumentPart.Document.Body;

        private string _docsPath { get; }

        public WordDocumentBuilder()
        {
            _docsPath = Path.Combine(AppContext.BaseDirectory, "tempDocs");

            if (!Directory.Exists(_docsPath))     // Create the log directory if it doesn't exist
                Directory.CreateDirectory(_docsPath);

            _memoryStream = new MemoryStream();
            _wordDocument = WordprocessingDocument.Create(_memoryStream, WordprocessingDocumentType.Document, true);
            MainDocumentPart mainPart = _wordDocument.AddMainDocumentPart();
            mainPart.Document = new Document();
            Document.Body = new Body();
            Document.Save(mainPart);
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
            Body.Append(p);
            Document.Save();
            //_wordDocument.Save();
        }

        public void BuildFooter()
        {
            Paragraph p = new Paragraph();
            ParagraphProperties pp = new ParagraphProperties();
            pp.Justification = new Justification() { Val = JustificationValues.Center };
            // Add paragraph properties to your paragraph
            p.Append(pp);
            // Run
            Run r = new Run();
            Text t = new Text($"Дата / Подпись") { Space = SpaceProcessingModeValues.Preserve };
            r.Append(t);
            p.Append(r);
            // Add your paragraph to docx body
            Body.Append(p);
            Document.Save();
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
            tc14.Append(p14);
            tr1.Append(tc14);


            foreach (var statementTransaction in statementTransactions)
            {
                TableRow tr = new TableRow();

                TableCell tc1 = new TableCell();
                Paragraph p1 = new Paragraph(new Run(new Text(statementTransaction.Date.ToLongDateString())));
                tc1.Append(p1);
                tr.Append(tc1);

                TableCell tc2 = new TableCell();
                Paragraph p2 = new Paragraph(new Run(new Text(statementTransaction.Amount.ToString())));
                tc2.Append(p2);
                tr.Append(tc2);

                TableCell tc3 = new TableCell();
                Paragraph p3 = new Paragraph(new Run(new Text(statementTransaction.Sender)));
                tc3.Append(p3);
                tr.Append(tc3);

                TableCell tc4 = new TableCell();
                Paragraph p4 = new Paragraph(new Run(new Text(statementTransaction.Recipient)));
                tc4.Append(p4);
                tr.Append(tc4);

                table.Append(tr);
            }

            /*
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
            */

            // Add your table to docx body
            Body.Append(table);
            Document.Save();
            //_wordDocument.Save();

        }

        public Stream GetResult()
        {
            //string path = Path.Combine(_docsPath, $"{Guid.NewGuid()}.docx");
            //_wordDocument.SaveAs(path);

            MemoryStream stream = new MemoryStream();
            _wordDocument.Clone(stream, false);

            //byte[] byteArray = File.ReadAllBytes(path);
            //MemoryStream stream = new MemoryStream(byteArray);

            //_memoryStream.Position = 0;
            //_memoryStream.CopyTo(stream);
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
