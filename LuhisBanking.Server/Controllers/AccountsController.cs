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
            var x = await Task.WhenAll(t.SelectMany(success =>
            {
                var (login, results) = success;
                return results.AsT0.results
                    .Select(a => (a, trueLayerService.GetAccountBalance(login, a.account_id, cancellationToken)))
                    .Select(AsyncTupleFunctions.Convert);
            }));

            var final = x.Select(ToDto);

            return new ActionResult<IReadOnlyList<AccountDto>>(final.ToList());
        }

        private static AccountDto ToDto((Account, (Login, OneOf.OneOf<Result<Balance>, Error>)) a)
        {
            var (acc, other) = a;
            var (_, b) = other;
            return b.Match(
                    success => new AccountDto(acc.account_id, acc.display_name, success.results.Single().available),
                    error => throw new Exception(error.error)
                );
        }
    }
}
