using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationServer
{
    public class JwtCoder
    {
        public string Issuer;
        public string Audience;
        private string SecretString;
        JwtSecurityTokenHandler _tokenHandler;
        SymmetricSecurityKey _key;
        SigningCredentials _cred;

        public JwtCoder (IConfiguration config)
        {
            Issuer = config["JwtIssuer"];
            Audience = config["JwtAudience"];
            SecretString = config["SigningKey"];
            _key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (SecretString));
            _cred = new SigningCredentials (_key, SecurityAlgorithms.HmacSha256);

            _tokenHandler = new JwtSecurityTokenHandler ();
        }

        public string Encode (string username, Dictionary<string, object> payload)
        {
            var token = new JwtSecurityToken (
                Issuer,
                Audience,
                new List<Claim> ()
                {
                    new Claim (ClaimTypes.NameIdentifier, Guid.NewGuid ().ToString ()),
                        new Claim (ClaimTypes.Name, "User")
                },
                DateTime.Now,
                DateTime.Now.AddHours (24),
                _cred);
            return _tokenHandler.WriteToken (token);
        }
    }
}