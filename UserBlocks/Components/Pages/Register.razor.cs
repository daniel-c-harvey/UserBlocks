using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using UserBlocks.Identity;
using UserBlocks.Components.Pages.Models;
using Microsoft.AspNetCore.Components.Forms;

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

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

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
}