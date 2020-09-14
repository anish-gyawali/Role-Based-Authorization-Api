using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AntsyProject_285.Data;
using AntsyProject_285.Features.Roles;
using AntsyProject_285.Features.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AntsyProject_285.Controllers
{
    [Route("api/createaccount")]
    [ApiController]
    public class CreateAccountController : ControllerBase
    {
        private readonly DataContext dataContext;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public CreateAccountController(DataContext dataContext, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.dataContext = dataContext;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await userManager.FindByIdAsync(userId);
            return new
            {
                user.Id,
                user.UserName,
                user.PhoneNumber,
                user.Email,
            };
        }
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser(CreateAccountDTO dto)
        {
            var newuser = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
            };

            using (var transaction = await dataContext.Database.BeginTransactionAsync())
            {
                var identityresults = await userManager.CreateAsync(newuser, dto.Password);

                if (!identityresults.Succeeded)
                {
                    return BadRequest();
                }

                var roleresults = await userManager.AddToRoleAsync(newuser, Roles.Customer);

                if (!roleresults.Succeeded)
                {
                    return BadRequest();
                }

                transaction.Commit();

                await signInManager.SignInAsync(newuser, isPersistent: false);

                return Created(string.Empty, new UserDTO
                {
                    UserName = newuser.UserName,
                    Email = newuser.Email,
                    PhoneNumber = newuser.PhoneNumber,
                });
            }
        }
    }
}