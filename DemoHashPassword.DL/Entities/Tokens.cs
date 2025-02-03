using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoHashPassword.DL.Entities
{
    public class Tokens
    {
        public string JWT { get; set; }
        public string RefreshToken { get; set; }

        public Tokens() { }

        public Tokens(string jWT, string refreshToken)
        {
            JWT = jWT;
            RefreshToken = refreshToken;
        }
    }
}
