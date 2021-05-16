using MCB.VBO.Microservices.Statements.Shared.Models;
using MCB.VBO.TemplatesLib.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCB.VBO.TemplatesLib
{
    public class DocumentDirector
    {
        public DocumentDirector()
        {

        }

        public void CreateWordDoc(WordDocumentBuilder builder, StatementData statement)
        {
            builder.BuildHeader(statement);
            builder.BuildTable(statement.StatementTransactions);
            builder.BuildFooter();
        }
    }
}
