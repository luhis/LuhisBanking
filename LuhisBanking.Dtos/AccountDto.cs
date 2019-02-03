using System;

namespace LuhisBanking.Dtos
{
    public class AccountDto
    {
        public AccountDto()
        {
        }

        public AccountDto(string accountId, string name, decimal balance)
        {
            AccountId = accountId;
            Name = name;
            Balance = balance;
        }

        public string AccountId { get; set; }

        public string Name { get; set; }

        public Decimal Balance { get; set; }
    }
}