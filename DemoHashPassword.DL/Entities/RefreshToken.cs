using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHashPassword.DL.Entities
{
    public class RefreshToken
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiredAt { get; set; }
        public bool Used { get; set; }


        public RefreshToken()
        {
        }

        public RefreshToken(int userId, string token, DateTime expiresAt, bool used = false)
        {
            UserId = userId;
            Token = token;
            ExpiredAt = expiresAt;
            Used = used;
        }
    }
}
