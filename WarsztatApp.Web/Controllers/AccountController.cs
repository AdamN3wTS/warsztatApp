﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WarsztatApp.Web.Data;
using WarsztatApp.Web.Models;

namespace WarsztatApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _appDbContext;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appDbContext = appDbContext;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid) 
            {
                var user = new IdentityUser { UserName = registerViewModel.Email, Email = registerViewModel.Email };
                var result = await _userManager.CreateAsync(user,registerViewModel.Password);
                if (result.Succeeded) 
                {
                    var warsztat = new Warsztat
                    {
                        Email = user.Email,
                        UserId = user.Id,
                        Magazyn = new Magazyn()
                    };
                    _appDbContext.Warsztaty.Add(warsztat);
                    await _appDbContext.SaveChangesAsync();
                    return RedirectToAction("Login");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerViewModel);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, true, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Home", "Warsztat");
                }
                else
                {
                    ModelState.AddModelError("INVALID_LOGIN_ATTEMPT", "Invalid to loggin attempt");
                    return View(loginViewModel);
                }

            }
            return View(loginViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
