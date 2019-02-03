using LuhisBanking.Services;
using Microsoft.AspNetCore.Mvc;

namespace LuhisBanking.Server.Controllers
{
    [Route("api/[controller]")]
    public class ConnectionsController : Controller
    {
        private readonly ITrueLayerService trueLayerService;

        public ConnectionsController(ITrueLayerService trueLayerService)
        {
            this.trueLayerService = trueLayerService;
        }

        public void GetAll()
        {

        }
    }
}