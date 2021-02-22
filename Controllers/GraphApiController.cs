using GraphiAPI.Models;
using GraphiAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GraphiAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GraphApiController : ControllerBase
    {
        private readonly ILogger<GraphApiController> _logger;

        private readonly IGraphService _graphService;

        public GraphApiController(ILogger<GraphApiController> logger, IGraphService graphService)
        {
            _logger = logger;
            _graphService = graphService;
        }

        [HttpGet("getuser/{id}")]
        public async Task<GraphUser> GetUserById([FromRoute] string id)
        {
            _logger.LogInformation("Calling GetUser");
            return await _graphService.GetUserById(id);
        }

        [HttpPost("createchat")]
        public async Task<Chat> CreateOneOnOneChat()
        {
            return await _graphService.CreateOneOnOneChat();
        }
    }
}
