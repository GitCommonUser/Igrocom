using Igrocom.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MychessProj.Pages.Account
{
    public class Hash_Cookie
    {
        public string HashPassword(string password)
        {
            string output;
            MD5 MD5Hash = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password); 
            byte[] hash = MD5Hash.ComputeHash(inputBytes); 
            output = Convert.ToHexString(hash);
            return output;
        }
        public ClaimsIdentity Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType,user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType,user.Role.ToString())
            };
            return new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}
