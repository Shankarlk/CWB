using CWB.App.AppUtils;
using CWB.App.Services.EmailServices;
using CWB.App.Services.EmployeeMaster;
using CWB.Constants.UserIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CWB.App.Controllers
{
    [Authorize(Roles = Roles.ADMIN)]
    public class ChangePasswordController : Controller
    {

        private readonly ILogger<ChangePasswordController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly KafkaEmailProducer _emailProducer;
        public ChangePasswordController(ILogger<ChangePasswordController> logger,IEmployeeService employeeService,
            KafkaEmailProducer emailProducer)
        {
            _logger = logger;
            _employeeService = employeeService;
            _emailProducer = emailProducer;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string Password,string CurrentPassword)
        {
            var result = await _employeeService.GetAllEmployee();
            ClaimsPrincipal userClaim = HttpContext.User;
            string fullName = AppUtil.GetUsername(userClaim);
            var model = result.Where(e => e.UserName == fullName).FirstOrDefault();
            if(model.Password != CurrentPassword)
            {
                return BadRequest(new { message = "Your Current Password is Incorrect!" });
            }
            model.ChangedPassword = 0;
            model.Password = Password;
            var Post = await _employeeService.PostEmployee(model);
            string plainText = $"Hi {model.Employee_name},\n\n" +
                               "Your Password has been Changed!\n" +
                               $"Your Username: {model.Email}\n" +
                               $"Your Changed Password: {model.Password}\n\n" +
                               "Thanks,\nKGK-Engineers";
            try
            {
                await _emailProducer.SendEmailRequestAsync(model.Email, "Your Password Has Changed!", plainText);
            }
            catch (Exception ex)
            {
            }
            return Ok(Post);
        }
        public static string GeneratePassword(int length = 10)
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "@#$%*!";

            Random random = new Random();

            // Ensure at least one of each required character
            string password =
                upper[random.Next(upper.Length)].ToString() +
                lower[random.Next(lower.Length)].ToString() +
                special[random.Next(special.Length)].ToString() +
                digits[random.Next(digits.Length)].ToString();

            // Remaining characters (mix of all types)
            string allChars = upper + lower + digits + special;
            int remaining = length - password.Length;
            password += new string(Enumerable.Range(0, remaining)
                               .Select(x => allChars[random.Next(allChars.Length)]).ToArray());

            // Shuffle the password so first chars aren't always in the same order
            return new string(password.OrderBy(c => random.Next()).ToArray());
        }
    }
}
