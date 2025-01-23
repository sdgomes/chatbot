using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Chatbot.Services
{
    public class OllamaService
    {
        private const string OllamaBaseUrl = "http://localhost:11434/api/generate";
        private readonly HttpClient _httpClient;
        private readonly EmbeddingService _embeddingService;
        private readonly RAGService _ragService;

        public OllamaService(HttpClient httpClient, EmbeddingService embeddingService, RAGService ragService)
        {
            _httpClient = httpClient;
            _embeddingService = embeddingService;
            _ragService = ragService;
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            // Passo 1: Obter o embedding da mensagem do usuário
            var userEmbedding = await _embeddingService.GetEmbeddingAsync(prompt);

            // Passo 2: Buscar o contexto relevante baseado no embedding
            var relevantContext = _ragService.GetRelevantContext(userEmbedding);

            string contextText = relevantContext ?? string.Empty;
            string fullPrompt = $"Contexto relevante: {contextText}\nPergunta: {prompt}";

            // Passo 5: Enviar o prompt para o Ollama API e obter a resposta
            var requestBody = new
            {
                model = "induscabos",
                prompt = fullPrompt,
                stream = false
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(OllamaBaseUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                return $"Erro ao chamar Ollama: {response.StatusCode}";
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

            return jsonResponse?.response ?? "Erro ao obter resposta";
        }
    }
}
