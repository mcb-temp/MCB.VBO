using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.IO;

namespace MCB.VBO.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] byteArray = File.ReadAllBytes("temp2.docx");
            using (MemoryStream mem = new MemoryStream())
            {
                mem.Write(byteArray, 0, (int)byteArray.Length);
                WordprocessingDocument wordDocument = WordprocessingDocument.Open(mem, true);
                //wordDocument.MainDocumentPart.Document.Body.

                Paragraph para = wordDocument.MainDocumentPart.Document.Body.AppendChild(new Paragraph());
                Run run = para.AppendChild(new Run());
                run.AppendChild(new Text("WoW This is interesting."));

                using (FileStream fileStream = new FileStream("temp3.docx", FileMode.Create))
                {
                    mem.WriteTo(fileStream);
                }
            }

            /*
            // Add a main document part. 
            MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();

            // Create the document structure and add some text.
            mainPart.Document = new Document();
            Body body = mainPart.Document.AppendChild(new Body());
            Paragraph para = body.AppendChild(new Paragraph());
            Run run = para.AppendChild(new Run());
            run.AppendChild(new Text("Create text in body - CreateWordprocessingDocument"));
            /*mainPart.Document = new Document(new Body());
            mainPart.Document.Save(mainPart);*/

            //mainPart.Document.Save();
            //wordDocument.Save();

            //wordDocument.SaveAs("temp2.docx");

            Console.WriteLine("Hello World!");
        }
    }
}
