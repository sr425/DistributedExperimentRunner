using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AuthenticationServer.Model;
using AuthenticationServer.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationServer.Controllers
{
    [Route ("authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private JwtCoder _coder;
        private UserManager<ApplicationUser> _userManager;

        public AuthenticationController (JwtCoder coder, UserManager<ApplicationUser> userManager)
        {
            _coder = coder;
            _userManager = userManager;
        }

        [HttpPost ("login")]
        public async Task<IActionResult> Login ([FromBody] CredentialViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest (ModelState);
            }

            var identity = await GetClaimsIdentity (credentials.Email, credentials.Password);
            if (identity == null)
            {
                return BadRequest (ModelState); //Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            var jwt = _coder.Encode (credentials.Email, null);
            return new OkObjectResult (jwt);
        }

        public ClaimsIdentity GenerateClaimsIdentity (string userName, string id)
        {
            return new ClaimsIdentity (new GenericIdentity (userName, "Token"), new []
            {
                new Claim ("id", id),
                    new Claim ("rol", "api_access")
            });
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity (string userName, string password)
        {
            if (string.IsNullOrEmpty (userName) || string.IsNullOrEmpty (password))
                return await Task.FromResult<ClaimsIdentity> (null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync (userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity> (null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync (userToVerify, password))
            {
                return await Task.FromResult (GenerateClaimsIdentity (userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity> (null);
        }
    }
}