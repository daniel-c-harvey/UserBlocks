using DataBlocks.DataAdapters;
using Microsoft.AspNetCore.Identity;

namespace UserBlocks.Identity;

public class CustomUserStore :
    IUserPasswordStore<ApplicationUser>, 
    IUserEmailStore<ApplicationUser>
{
    private IDataAdapter<ApplicationUser> _userAdapter;

    public CustomUserStore(IDataAdapter<ApplicationUser> userAdapter)
    {
        _userAdapter = userAdapter;
    }

    // IUserStore implementation
    public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        // Insert user into your database
        await _userAdapter.Insert(user);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        // Delete user from your database
        await _userAdapter.Delete(user);
        return IdentityResult.Success;
    }

    public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        // Find user by ID in your database
        if (!long.TryParse(userId, out long id)) throw new ArgumentException("Invalid user id");
        var result = await _userAdapter.GetByID(id);
        
        if (!result.Success || result.Value is null) throw new ArgumentException("Failed to query user by ID");
        return result.Value;
    }

    public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        // Find user by normalized username in your database
        var result = await _userAdapter.GetByPredicate(u => u.NormalizedUserName == normalizedUserName);
        if (!result.Success || result.Value is null || !result.Value.Any()) throw new ArgumentException("Failed to query user by name");
        return result.Value.First();
    }

    // IUserPasswordStore implementation
    public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.PasswordHash != null);
    }

    public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    // IUserEmailStore implementation
    public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
    {
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        var result = await _userAdapter.GetByPredicate(u => u.NormalizedEmail == normalizedEmail);
        if (!result.Success || result.Value is null || !result.Value.Any()) throw new ArgumentException("Failed to query user by email");
        return result.Value.First();
    }

    public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
    {
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    // Other IUserStore implementation
    public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        await _userAdapter.Update(user);
        return await Task.FromResult(IdentityResult.Success);
    }

    public void Dispose()
    {
        // _userAdapter?.Dispose();
    }
}