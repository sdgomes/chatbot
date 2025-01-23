using Chatbot.DTO;
using Chatbot.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly OllamaService _ollamaService;

        public ChatController(OllamaService ollamaService)
        {
            _ollamaService = ollamaService;
        }

        [HttpPost]
        public async Task<IActionResult> GetChatResponse([FromBody] ChatRequestDTO chat)
        {
            if (chat == null || string.IsNullOrWhiteSpace(chat.Message))
            {
                return BadRequest("O prompt não pode estar vazio.");
            }

            var response = await _ollamaService.GetResponseAsync(chat.Message);
            return Ok(new { response });
        }
    }
}