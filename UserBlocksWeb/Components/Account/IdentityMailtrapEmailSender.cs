using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetBlocks.Models.Environment;
using RazorCore.Email;
using UserBlocks.DataModels.Identity;

namespace UserBlocksWeb.Components.Account;

public class IdentityMailtrapEmailSender(
    // IOptions<AuthMessageSenderOptions> optionsAccessor,
    IOptions<EmailConnections> connection,
    ILogger<MailtrapEmailSender> logger) 
    : MailtrapEmailSender(connection, logger), IEmailSender<ApplicationUser>
{
    public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
        SendEmailAsync(email, user.UserName, "Confirm your email",
            $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
        SendEmailAsync(email, user.UserName, "Reset your password",
            $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
        SendEmailAsync(email, user.UserName, "Reset your password",
            $"Please reset your password using the following code: {resetCode}");
}