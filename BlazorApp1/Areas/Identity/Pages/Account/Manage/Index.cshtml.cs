// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BlazorApp1.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorApp1.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        [TempData]
        public string StatusMessage { get; set; }
        
        [BindProperty]
        public InputModel Input { get; set; }
        
        public class InputModel
        {
            [Required]
            [Display(Name = "Новое имя")]
            [StringLength(30, ErrorMessage = "Имя не может быть длиней чем 30 символов.")]
            public string NewUserName { get; set; }
        }
        
        [Display(Name = "Текущее имя")]
        public string Username { get; set; }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            Input = new InputModel
            {
                NewUserName = userName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не удалось найти пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не удалось найти пользователя с ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            
            if (Input.NewUserName != user.UserName)
            {
                var sameNameUser = _userManager.FindByNameAsync(Input.NewUserName).Result;
                if (sameNameUser != null)
                {
                    StatusMessage = "Имя зането!";
                    return RedirectToPage();
                }
                
                var request = await _userManager.SetUserNameAsync(user, Input.NewUserName);
                if (!request.Succeeded)
                {
                    StatusMessage = "Что-то пошло не так.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Профиль успешно обновлён!";
            return RedirectToPage();
        }
    }
}
