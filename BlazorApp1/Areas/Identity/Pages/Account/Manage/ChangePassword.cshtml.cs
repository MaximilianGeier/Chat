// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BlazorApp1.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BlazorApp1.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ChangePasswordModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [BindProperty]
        public InputModel Input { get; set; }
        
        [TempData]
        public string StatusMessage { get; set; }
        
        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Текущий пароль")]
            public string OldPassword { get; set; }
            
            [Required]
            [StringLength(100, ErrorMessage = "Пароль должен быть от {2} и до {1} символов", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Новый пароль")]
            public string NewPassword { get; set; }
            
            [DataType(DataType.Password)]
            [Display(Name = "Подтвердить пароль")]
            [Compare("NewPassword", ErrorMessage = "Пароль не совпадает")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не удалось найти пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не удалось найти пользователя с ID '{_userManager.GetUserId(User)}'.");
            }
            
            bool isPasswordCorrect = _userManager.CheckPasswordAsync(user, Input.OldPassword).Result;
            if (!isPasswordCorrect)
            {
                StatusMessage = "Пароль неверный!";
                return RedirectToPage();
            }
            
            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                StatusMessage = "Не удалось изменить пароль!";
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Пароль успешно изменён";

            return RedirectToPage();
        }
    }
}
