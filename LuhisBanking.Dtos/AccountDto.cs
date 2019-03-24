namespace LuhisBanking.Dtos
{
    public class AccountDto
    {
        public AccountDto()
        {
        }

        public AccountDto(string accountId, string name, decimal balance)
        {
            this.AccountId = accountId;
            this.Name = name;
            this.Balance = balance;
        }

        public string AccountId { get; set; }

        public string Name { get; set; }

        public decimal Balance { get; set; }
    }
}