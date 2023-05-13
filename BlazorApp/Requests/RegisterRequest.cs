﻿using System.ComponentModel.DataAnnotations;

namespace Chat.Requests;

public class RegisterRequest
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match!")]
    public string PasswordConfirm { get; set; }
}