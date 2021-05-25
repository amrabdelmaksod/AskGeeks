using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UserManagementWithIdentity.Models;
using UserManagementWithIdentity.ViewModels;

namespace UserManagementWithIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.Users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            })

                .ToListAsync();
            return View(user);
        }

       
        public async Task<IActionResult> Add()
        {
            var roles = await _roleManager.Roles.Select(r=> new RoleViewModel { RoleId = r.Id,RoleName = r.Name }).ToListAsync();
            var viewModel = new AddUserViewModel()
            {
                Roles = roles


            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {

            if (!ModelState.IsValid)
                return View(model);

            if (!model.Roles.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Roles", "Please select at least one role ");
                return View(model);
            }

            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Email is already exists!");
                return View(model);
            }

            if(await _userManager.FindByNameAsync(model.UserName) != null)
            {
                ModelState.AddModelError("UserName", "Username is already exists");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
               FirstName = model.FirstName,
               LastName = model.LastName,

            };

            var result = await _userManager.CreateAsync(user, model.Password);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Roles", error.Description);
                }
                return View(model);
            }

            await _userManager.AddToRolesAsync(user, model.Roles.Where(r=>r.IsSelected).Select(r=>r.RoleName));

            return RedirectToAction(nameof(Index));
            
           
        }

        public async Task<IActionResult> Edit(string userId)
        {
  
              
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var viewModel = new ProfileFormViewModel
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
               
            };
            return View(viewModel);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            /* Important*/
            /* لازم اتأكد ان اليوزر لما هيغير الايميل لو الايميل دة موجود اصلا مع يوزر تاني يبقى اديله ايرور بسيط و افهمه ان الايميل دة مع يوزر تاني  */

            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

            if(userWithSameEmail!=null && userWithSameEmail.Id != model.Id)
            {
                ModelState.AddModelError("Email", "Sorry this emails is assigned to another user!");
                return View(model);
            }

            var userWithSameUserName = await _userManager.FindByNameAsync(model.UserName);
            if(userWithSameUserName != null && userWithSameUserName.Id != model.Id)
            {
                ModelState.AddModelError("UserName", "Sorry this username is assigned to another user");
                return View(model);
            }

           


            user.Id = model.Id;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
               
        }

        [HttpGet]
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var roles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return NotFound();
            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
            }
            return RedirectToAction(nameof(Index));
        }




    }
}
