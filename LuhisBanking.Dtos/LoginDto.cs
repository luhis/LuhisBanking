using System;

namespace LuhisBanking.Dtos
{
    public class LoginDto
    {
        public LoginDto()
        {

        }

        public LoginDto(Guid id, string displayName)
        {
            this.Id = id;
            this.DisplayName = displayName;
        }

        public Guid Id { get; set; }

        public string DisplayName { get; set; }
    }
}