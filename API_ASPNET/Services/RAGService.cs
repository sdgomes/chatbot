using Chatbot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot.Services
{
    public class RAGService
    {
        private readonly List<DocumentEmbedding> _documentEmbeddings;
        private readonly List<Document> _documents;

        public RAGService(List<DocumentEmbedding> documentEmbeddings, List<Document> documents)
        {
            _documentEmbeddings = documentEmbeddings;
            _documents = documents;
        }

        public string GetRelevantContext(List<float> queryEmbedding)
        {
            // Encontrar o documento mais relevante usando similaridade do cosseno
            var bestMatch = _documentEmbeddings
                .Select(de => new
                {
                    DocumentId = de.DocumentId,
                    Similarity = CosineSimilarity(queryEmbedding, de.Embedding)
                })
                .OrderByDescending(x => x.Similarity)
                .FirstOrDefault();

            if (bestMatch != null)
            {
                // Recuperar o texto do documento mais relevante
                var relevantDocument = _documents.FirstOrDefault(doc => doc.Id == bestMatch.DocumentId);
                return relevantDocument?.Text; // Retorna o texto do documento relevante
            }

            return null; // Caso não encontre um contexto relevante
        }

        private double CosineSimilarity(List<float> vec1, List<float> vec2)
        {
            // Implementação simplificada de similaridade do cosseno
            double dotProduct = vec1.Zip(vec2, (a, b) => a * b).Sum();
            double magnitude1 = Math.Sqrt(vec1.Sum(v => v * v));
            double magnitude2 = Math.Sqrt(vec2.Sum(v => v * v));
            return dotProduct / (magnitude1 * magnitude2);
        }

    }
}
