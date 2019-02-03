using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult<IReadOnlyList<AccountDto>>> GetAll()
        {
            var t = await trueLayerService.GetAccounts();
            var errors = t.Where(a => a.IsT1).Select(a =>a.AsT1).ToList();
            if (errors.Any())
            {
                throw new Exception(string.Join(", ", errors.Select(a => a.error)));
            }
            var final = t.Select(a => a.AsT0).SelectMany(success =>
            {
                var (login, results) = success;
                var tasks = results.results
                    .Select(a => (a, trueLayerService.GetAccountBalance(login, a.account_id)))
                    .Select(AsyncTupleFunctions.Convert);
                var res = Task.WhenAll(tasks).Result; //todo
                var x = res.Select(ToDto).ToList();
                return x;
            });

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
