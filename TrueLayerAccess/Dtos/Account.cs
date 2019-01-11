using System;

namespace TrueLayerAccess.Dtos
{
    public class Account
    {
        public Account()
        {
        }

        public Account(DateTime update_timestamp, string account_id, string account_type, string display_name, string currency, AccountNumber account_number, Provider provider)
        {
            this.update_timestamp = update_timestamp;
            this.account_id = account_id;
            this.account_type = account_type;
            this.display_name = display_name;
            this.currency = currency;
            this.account_number = account_number;
            this.provider = provider;
        }

        public DateTime update_timestamp { get; set; }
        public string account_id { get; set; }
        public string account_type { get; set; }
        public string display_name { get; set; }
        public string currency { get; set; }
        public AccountNumber account_number { get; set; }
        public Provider provider { get; set; }
    }
}