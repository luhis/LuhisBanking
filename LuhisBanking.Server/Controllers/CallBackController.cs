using System;
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

        public CallBackController(IOptions<MyAppSettings> settings, ILogger<CallBackController> logger)
        {
            this.logger = logger;
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
                
                Response.Cookies.SetAccessToken(t.access_token);
                Response.Cookies.SetRefreshToken(t.refresh_token);
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