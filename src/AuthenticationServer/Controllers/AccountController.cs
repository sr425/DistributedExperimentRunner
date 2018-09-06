using System.Threading.Tasks;
using AuthenticationServer.Model;
using AuthenticationServer.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace AuthenticationServer.Controllers
{
    [Route ("accounts/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        private IMapper _mapper;

        public AccountController (ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            //_mapper = mapper;
            var config = new MapperConfiguration (cfg => cfg.CreateMap<RegisterViewModel, ApplicationUser> (MemberList.Source));
            _mapper = config.CreateMapper ();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register ([FromBody] RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);

            var user = _mapper.Map<ApplicationUser> (viewModel);
            user.UserName = user.Email;
            var result = await _userManager.CreateAsync (user, viewModel.Password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError (string.Empty, result.ToString ());
                return BadRequest (ModelState);
            }
            return Ok ();
        }
    }
}