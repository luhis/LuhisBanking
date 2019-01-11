using System.Threading.Tasks;
using LuhisBanking.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TrueLayerAccess;
using TrueLayerAccess.Dtos;
using WebApplication1.Server;

namespace LuhisBanking.Server.Controllers
{
    [Route("api/[controller]")]
    public class CallBackController : Controller
    {
        private readonly MyAppSettings settings;

        public CallBackController(IOptions<MyAppSettings> settings)
        {
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
            var r = new CallBackResult(code, scope.Split(' '));
            var t = await TrueLayerAuthApi.GetAuthToken(new TokenRequest(settings.ClientId, settings.ClientSecret, r.Code, GetRedirectUrl()));
            
            Response.Cookies.SetAccessToken(t.access_token);
            Response.Cookies.SetRefreshToken(t.refresh_token);
            return Redirect(Url.Content("~/"));
        }

        [HttpGet("[action]")]
        public string GetAuthLink()
        {
            return TrueLayerAuthApi.AuthLink(GetRedirectUrl(), settings.ClientId);
        }
    }
}