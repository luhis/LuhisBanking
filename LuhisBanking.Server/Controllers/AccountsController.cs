using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LuhisBanking.Dtos;
using LuhisBanking.Services;
using Microsoft.AspNetCore.Mvc;
using TrueLayerAccess.Dtos;

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
        public async Task<ActionResult<IReadOnlyList<AccountDto>>> GetAll(CancellationToken cancellationToken)
        {
            var t = await trueLayerService.GetAccounts(cancellationToken);
            var errors = t.ExtractErrors();
            if (errors.Any())
            {
                throw new Exception(string.Join(", ", errors.Select(a => a.error)));
            }
            var x = await Task.WhenAll(t.Select(a => a.AsT0).SelectMany(success =>
            {
                var (login, results) = success;
                return results.results
                    .Select(a => (a, trueLayerService.GetAccountBalance(login, a.account_id, cancellationToken)))
                    .Select(AsyncTupleFunctions.Convert);
            }));

            var final = x.Select(ToDto);

            return new ActionResult<IReadOnlyList<AccountDto>>(final.ToList());
        }

        private static AccountDto ToDto((Account, OneOf.OneOf<(Login, Result<Balance>), Error>) a)
        {
            var (acc, bal) = a;
            return bal.Match(
                    success => new AccountDto(acc.account_id, acc.display_name, success.Item2.results.Single().available),
                    error => throw new Exception(error.error)
                );
        }
    }
}
