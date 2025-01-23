using Chatbot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Services
{
    public class EmbeddingService
    {
        private readonly HttpClient _httpClient;
        private const string FlaskBaseUrl = "http://localhost:5002/embed";  // URL do servidor Flask

        public EmbeddingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<float>> GetEmbeddingAsync(string text)
        {
            var requestBody = new { text };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(FlaskBaseUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao chamar o serviço de embedding: {response.StatusCode}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<EmbeddingResponse>(responseString);

            return jsonResponse?.Embedding ?? new List<float>();
        }
    }
}
