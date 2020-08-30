using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Jotaro.Controllers
{
    [ApiController]
    [Route("api/post")]
    public class PostController: ControllerBase
    {
        private readonly ILogger logger;

        public PostController(ILogger<PostController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(/*Post post*/)
        {
            await Task.Delay(0).ConfigureAwait(false);
            logger.LogInformation("Hello, world!");
            return Ok();
        }
    }
}
