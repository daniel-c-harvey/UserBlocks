using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using UserBlocks.Identity;

namespace UserBlocks.Components.Pages;

public partial class Register : ComponentBase
{
    [Inject]
    public required UserManager<ApplicationUser> UserManager { get; set; }
    [Inject]
    public required SignInManager<ApplicationUser> SignInManager { get; set; }
    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private RegisterModel registerModel = new();
    private string errorMessage = string.Empty;

    private async Task HandleRegistration()
    {
        var user = new ApplicationUser
        {
            UserName = registerModel.Email,
            Email = registerModel.Email
        };

        var result = await UserManager.CreateAsync(user, registerModel.Password);
        
        if (result.Succeeded)
        {
            await SignInManager.SignInAsync(user, isPersistent: false);
            NavigationManager.NavigateTo("/");
        }
        else
        {
            foreach (var error in result.Errors)
            {
                errorMessage += $"{error.Description} ";
            }
            StateHasChanged();
        }
    }

    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}