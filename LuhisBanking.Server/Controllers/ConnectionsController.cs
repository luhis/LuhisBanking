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
    public class ConnectionsController : Controller
    {
        private readonly ITrueLayerService trueLayerService;

        public ConnectionsController(ITrueLayerService trueLayerService)
        {
            this.trueLayerService = trueLayerService;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<LoginDto>> GetAll(CancellationToken cancellationToken)
        {
            var r = await this.trueLayerService.GetLogins(cancellationToken);
            var errors = r.ExtractErrors();
            if (errors.Any())
            {
                throw new Exception(string.Join(", ", errors.Select(a => a.error)));
            }

            return r.Select(a => a.AsT0).Select(a =>
            {
                var (login, result) = a;
                return new LoginDto(login.Id, result.results.First().provider.display_name);
            });
        }
    }
}