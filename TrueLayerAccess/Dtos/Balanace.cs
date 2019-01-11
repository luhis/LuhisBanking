using System;

namespace TrueLayerAccess.Dtos
{
    public class Balance
    {
        public string currency { get; set; }
        public decimal available { get; set; }
        public decimal current { get; set; }
        public int overdraft { get; set; }
        public DateTime update_timestamp { get; set; }
    }
}