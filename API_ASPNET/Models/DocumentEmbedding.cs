using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot.Models
{
    public class DocumentEmbedding
    {
        public string DocumentId { get; set; }

        public List<float> Embedding { get; set; }
    }
}
