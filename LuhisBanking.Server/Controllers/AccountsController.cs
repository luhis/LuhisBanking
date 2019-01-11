using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LuhisBanking.Services;
using Microsoft.AspNetCore.Mvc;
using TrueLayerAccess.Dtos;
using WebApplication1.Server;

namespace LuhisBanking.Server.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly ITrueLayerService trueLayerService;

        public AccountsController(ITrueLayerService trueLayerService)
        {
            this.trueLayerService = trueLayerService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IReadOnlyList<AccountDto>>> GetAll()
        {
            var token = this.Request.Cookies.GetAccessToken();
            var refreshToken = this.Request.Cookies.GetRenewalToken();
            if (!string.IsNullOrEmpty(token))
            {
                var tokens = new Tokens(token, refreshToken);
                var t = await trueLayerService.GetAccounts(tokens);
                return await t.Match(async success =>
                {
                    var tasks = success.results
                        .Select(a => (a, trueLayerService.GetAccountBalance(tokens, a.account_id)))
                        .Select(AsyncTupleFunctions.Convert);
                    var res = await Task.WhenAll(tasks);
                    return new ActionResult<IReadOnlyList<AccountDto>>(res.Select(ToDto).ToList());

                }, error => throw new Exception(error.error));
            }
            else
            {
                return new AccountDto[]{ };
            }
        }

        private static AccountDto ToDto((Account, OneOf.OneOf<Result<Balance>, Error>) a)
        {
            var (acc, bal) = a;
            return bal.Match(
                    success => new AccountDto(acc.account_id, acc.display_name, success.results.Single().available),
                    error => throw new Exception(error.error)
                );
        }
    }
}
