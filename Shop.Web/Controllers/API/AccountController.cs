namespace Shop.Web.Controllers.API
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using Data;
    using Helpers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[Controller]")]
    public class AccountController : Controller
    {
        private readonly IUserHelper userHelper;
        private readonly ICountryRepository countryRepository;
        private readonly IMailHelper mailHelper;

        public AccountController(
            IUserHelper userHelper,
            ICountryRepository countryRepository,
            IMailHelper mailHelper)
        {
            this.userHelper = userHelper;
            this.countryRepository = countryRepository;
            this.mailHelper = mailHelper;
        }

        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] NewUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.userHelper.GetUserByEmailAsync(request.Email);
            if (user != null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "This email is already registered."
                });
            }

            var city = await this.countryRepository.GetCityAsync(request.CityId);
            if (city == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "City don't exists."
                });
            }

            user = new Data.Entities.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                Address = request.Address,
                PhoneNumber = request.Phone,
                CityId = request.CityId,
                City = city
            };

            var result = await this.userHelper.AddUserAsync(user, request.Password);
            if (result != IdentityResult.Success)
            {
                return this.BadRequest(result.Errors.FirstOrDefault().Description);
            }

            var myToken = await this.userHelper.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = this.Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            this.mailHelper.SendMail(request.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                $"To allow the user, " +
                $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "A Confirmation email was sent. Plese confirm your account and log into the App."
            });
        }

        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            var user = await this.userHelper.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return this.BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "This email is not assigned to any user."
                });
            }

            var myToken = await this.userHelper.GeneratePasswordResetTokenAsync(user);
            var link = this.Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
            this.mailHelper.SendMail(request.Email, "Password Reset", $"<h1>Recover Password</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

            return Ok(new Response
            {
                IsSuccess = true,
                Message = "An email with instructions to change the password was sent."
            });
        }
    }
}