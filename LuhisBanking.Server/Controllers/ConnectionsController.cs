using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LuhisBanking.Dtos;
using LuhisBanking.Services;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using TrueLayerAccess.Dtos;

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

        private static string ToString(OneOf<Result<MetaData>, Error> r)
        {
            return r.Match(a => a.results.Single().provider.display_name, e => e.error);
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<LoginDto>> GetAll(CancellationToken cancellationToken)
        {
            var r = await this.trueLayerService.GetLogins(cancellationToken);

            return r.Select(a =>
            {
                var (login, result) = a;

                return new LoginDto(login.Id, ToString(result));
            });
        }

        [HttpPost("[action]/{id}")]
        public Task Delete(Guid id, CancellationToken cancellationToken)
        {
            return this.trueLayerService.DeleteLogin(id, cancellationToken);
        }
    }
}