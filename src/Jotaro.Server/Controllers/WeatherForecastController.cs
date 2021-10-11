using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Jotaro.Server.Controllers;

[ApiController, Route("[controller]")]
public class OneBotController : ControllerBase
{
    private readonly ILogger<OneBotController> logger;

    public OneBotController(ILogger<OneBotController> logger) => this.logger = logger;
}
