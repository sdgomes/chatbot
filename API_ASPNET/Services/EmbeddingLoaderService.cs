using Chatbot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Chatbot.Services
{
    public class EmbeddingLoaderService
    {
        private readonly string _embeddingsFilePath = @"D:\AnexosPortais\Portal\Learning\embeddings.json"; // Caminho do JSON
        private readonly string _documentsPath = @"D:\AnexosPortais\Portal\Learning\Documentos"; // Diretório dos textos

        public async Task<(List<DocumentEmbedding>, List<Document>)> LoadEmbeddingsAndDocumentsAsync()
        {
            var documentEmbeddings = new List<DocumentEmbedding>();
            var documents = new List<Document>();

            // Verifica se o JSON de embeddings existe
            if (File.Exists(_embeddingsFilePath))
            {
                var json = await File.ReadAllTextAsync(_embeddingsFilePath);
                var embeddingsDict = JsonConvert.DeserializeObject<Dictionary<string, List<float>>>(json);

                foreach (var kvp in embeddingsDict)
                {
                    documentEmbeddings.Add(new DocumentEmbedding
                    {
                        DocumentId = kvp.Key,
                        Embedding = kvp.Value
                    });
                }
            }

            // Carregar textos dos documentos
            if (Directory.Exists(_documentsPath))
            {
                var files = Directory.GetFiles(_documentsPath, "*.txt");

                foreach (var file in files)
                {
                    var text = await File.ReadAllTextAsync(file);
                    var docId = Path.GetFileNameWithoutExtension(file); // Usa o nome do arquivo como ID
                    documents.Add(new Document
                    {
                        Id = docId,
                        Text = text
                    });
                }
            }

            return (documentEmbeddings, documents);
        }
    }
}
