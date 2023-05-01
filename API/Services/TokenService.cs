
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {


        private readonly SymmetricSecurityKey _key;
        // Implicit importing hence no need of adding using namespace for IConfiguration here
        //Symmetric key means same key is used to encrypt and decrypt the key
        //ASymmetric key is public and private like as in https connection
        private readonly UserManager<AppUser> _userManager;
        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            //TokenKey is stored in appsetting.json. This is how values are fetched from appsetting.json
            // Symmetric security key takes a byte array             
            _key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config["TokenKey"]));
            _userManager = userManager;
        }
        public async Task<string> CreateToken(AppUser user)
        {

            //Claims is the data inside token
            var claims = new List<Claim>
            {
                //Add all claims need here
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),

            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


            //signing key for jwt token
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);


            //3 parts ok jwt token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            //token descriptor is passed to tokenhandler which gives the token finally
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}