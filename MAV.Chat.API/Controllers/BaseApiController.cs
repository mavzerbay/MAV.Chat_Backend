using MAV.Chat.Common.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MAV.Chat.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController : ControllerBase
    {

    }
}
