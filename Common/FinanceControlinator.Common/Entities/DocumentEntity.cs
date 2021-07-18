using System;

namespace FinanceControlinator.Common.Entities
{
    public interface IDocumentEntity
    {
        public String Id { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }

    public class DocumentEntity : IDocumentEntity
    {
        public String Id { get; set; }

        public DateTime InsertDate { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
