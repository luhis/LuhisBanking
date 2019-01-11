namespace TrueLayerAccess.Dtos
{
    public class AccountNumber
    {
        public AccountNumber()
        {
        }

        public string iban { get; set; }
        public string number { get; set; }
        public string sort_code { get; set; }
        public string swift_bic { get; set; }
    }
}