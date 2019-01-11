using System.IO;
using FluentAssertions;
using Microsoft.JSInterop;
using TrueLayerAccess.Dtos;
using Xunit;

namespace TrueLayer.Api.Test.Unit
{
    public class DeserialiserShould
    {
        [Fact]
        public void DeserialiseAccounts()
        {
            var r = Json.Deserialize<Result<Account>>(File.ReadAllText("../../../Json/Accounts.json"));

            r.Should().NotBeNull();
            r.results.Length.Should().Be(2);
        }
        
        [Fact]
        public void DeserialiseTransactions()
        {
            var r = Json.Deserialize<Result<Transaction>>(File.ReadAllText("../../../Json/Transactions.json"));

            r.Should().NotBeNull();
            r.results.Length.Should().Be(2);
        }

        [Fact]
        public void DeserialiseBalance()
        {
            var r = Json.Deserialize<Result<Balance>>(File.ReadAllText("../../../Json/Balance.json"));

            r.Should().NotBeNull();
            r.results.Length.Should().Be(1);
        }

        [Fact]
        public void DeserialiseTokenResponse()
        {
            var r = Json.Deserialize<TokenResponse>(File.ReadAllText("../../../Json/TokenResponse.json"));

            r.Should().NotBeNull();
            r.expires_in.Should().Be(3600);
        }

        [Fact]
        public void ErrorResponse()
        {
            var r = Json.Deserialize<Error>(File.ReadAllText("../../../Json/Error.json"));

            r.Should().NotBeNull();
            r.error_details.detail_key.Should().Be("detail value");
        }
    }
}
