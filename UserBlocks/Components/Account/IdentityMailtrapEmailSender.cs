using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RazorCore.Email;
using UserBlocks.Identity;

namespace UserBlocks.Components.Account;

public class IdentityMailtrapEmailSender(
    IOptions<AuthMessageSenderOptions> optionsAccessor, 
    ILogger<MailtrapEmailSender> logger) 
    : MailtrapEmailSender(optionsAccessor, logger), IEmailSender<ApplicationUser>
{
    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
        SendEmailAsync(email, "Confirm your email",
            $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
        SendEmailAsync(email, "Reset your password",
            $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
        SendEmailAsync(email, "Reset your password",
            $"Please reset your password using the following code: {resetCode}");
}