using System;
using System.Threading;
using System.Threading.Tasks;
using LuhisBanking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TrueLayerAccess;
using TrueLayerAccess.Dtos;

namespace LuhisBanking.Server.Controllers
{
    [Route("api/[controller]")]
    public class CallBackController : Controller
    {
        private readonly ILogger<CallBackController> logger;
        private readonly MyAppSettings settings;
        private readonly ILoginsRepository loginsRepository;

        public CallBackController(IOptions<MyAppSettings> settings, ILogger<CallBackController> logger, ILoginsRepository loginsRepository)
        {
            this.logger = logger;
            this.loginsRepository = loginsRepository;
            this.settings = settings.Value;
        }

        private string GetRedirectUrl()
        {
            return this.Url.Action(
                nameof(ProcessCallBack),
                this.ControllerContext.ActionDescriptor.ControllerName,
                null,
                this.HttpContext.Request.Scheme,
                this.HttpContext.Request.Host.ToString());
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> ProcessCallBack(string code, string scope)
        {
            try
            {
                var r = new CallBackResult(code, scope.Split(' '));
                var t = await TrueLayerAuthApi.GetAuthToken(new TokenRequest(settings.ClientId, settings.ClientSecret, r.Code, GetRedirectUrl()));
                await loginsRepository.Add(new Login(Guid.NewGuid(), t.access_token, t.refresh_token, DateTime.UtcNow), CancellationToken.None);
            }
            catch (Exception e)
            {
                logger.LogError("Callback call failed", e);
            }

            return Redirect(Url.Content("~/"));
        }

        [HttpGet("[action]")]
        public string GetAuthLink()
        {
            return TrueLayerAuthApi.AuthLink(GetRedirectUrl(), settings.ClientId);
        }
    }
}