using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using UserBlocks.Identity;

namespace UserBlocks.Components.Pages;

public partial class Login : ComponentBase
{
    [Inject] 
    public NavigationManager NavigationManager { get; set; }
    [Inject]
    public SignInManager<ApplicationUser> SignInManager { get; set; }
    private LoginModel loginModel = new();
    private string errorMessage = string.Empty;
    private string? returnUrl = null;

    protected override void OnInitialized()
    {
        // Extract the return URL from the query string
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out var url))
        {
            returnUrl = url;
        }
    }

    private async Task HandleLogin()
    {
        var result = await SignInManager.PasswordSignInAsync(
            loginModel.Email, 
            loginModel.Password, 
            loginModel.RememberMe, 
            lockoutOnFailure: false);
            
        if (result.Succeeded)
        {
            NavigationManager.NavigateTo(returnUrl ?? "/");
        }
        else
        {
            errorMessage = "Invalid login attempt.";
            StateHasChanged();
        }
    }

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}