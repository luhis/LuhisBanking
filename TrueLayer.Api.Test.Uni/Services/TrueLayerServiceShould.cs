using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using LuhisBanking.Services;
using Microsoft.Extensions.Options;
using Moq;
using OneOf;
using TrueLayerAccess;
using TrueLayerAccess.Dtos;
using Xunit;

namespace TrueLayer.Api.Test.Unit.Services
{
    public class TrueLayerServiceShould
    {
        [Fact]
        public void GetNoLogins()
        {
            var mr = new MockRepository(MockBehavior.Strict);
            var loginRepository = mr.Create<ILoginsRepository>();
            loginRepository.Setup(a => a.GetAll(CancellationToken.None))
                .Returns(Task.FromResult<IReadOnlyList<Login>>(new List<Login>()));
            var trueLayer = mr.Create<ITrueLayerApi>();
            var trueLayerAuth = mr.Create<ITrueLayerAuthApi>();
            var svc = (ITrueLayerService) new TrueLayerService(
                Options.Create(new MyAppSettings()), loginRepository.Object, trueLayer.Object, trueLayerAuth.Object);
            var r = svc.GetLogins(CancellationToken.None).Result;
            r.Should().NotBeNull();
            mr.VerifyAll();
        }

        [Fact]
        public void GetLogins()
        {
            var mr = new MockRepository(MockBehavior.Strict);
            var loginRepository = mr.Create<ILoginsRepository>();
            loginRepository.Setup(a => a.GetAll(CancellationToken.None)).Returns(Task.FromResult<IReadOnlyList<Login>>(
                new List<Login>()
                {
                    new Login(Guid.NewGuid(), "access", "refresh", DateTime.MinValue)
                }));
            var trueLayer = mr.Create<ITrueLayerApi>();
            trueLayer.Setup(a => a.GetMetaData("access")).Returns(
                Task.FromResult<OneOf<Result<MetaData>, Unauthorised, Error>>(
                    new Result<MetaData>(new[] {new MetaData(),})));
            var trueLayerAuth = mr.Create<ITrueLayerAuthApi>();
            var svc = (ITrueLayerService) new TrueLayerService(
                Options.Create(new MyAppSettings()), loginRepository.Object, trueLayer.Object, trueLayerAuth.Object);
            var r = svc.GetLogins(CancellationToken.None).Result;
            r.Should().NotBeNull();
            var (_, result) = r.First();
            result.IsT0.Should().BeTrue();
            mr.VerifyAll();
        }

        [Fact]
        public void GetLoginsFail()
        {
            var mr = new MockRepository(MockBehavior.Strict);
            var loginRepository = mr.Create<ILoginsRepository>();
            loginRepository.Setup(a => a.GetAll(CancellationToken.None)).Returns(Task.FromResult<IReadOnlyList<Login>>(
                new List<Login>()
                {
                    new Login(Guid.NewGuid(), "access", "refresh", DateTime.MinValue)
                }));
            var trueLayer = mr.Create<ITrueLayerApi>();
            trueLayer.Setup(a => a.GetMetaData("access")).Returns(
                Task.FromResult<OneOf<Result<MetaData>, Unauthorised, Error>>(
                    new Error()));
            var trueLayerAuth = mr.Create<ITrueLayerAuthApi>();
            var svc = (ITrueLayerService)new TrueLayerService(
                Options.Create(new MyAppSettings()), loginRepository.Object, trueLayer.Object, trueLayerAuth.Object);
            var r = svc.GetLogins(CancellationToken.None).Result;
            r.Should().NotBeNull();
            var (_, result) = r.First();
            result.IsT1.Should().BeTrue();
            mr.VerifyAll();
        }
    }
}
