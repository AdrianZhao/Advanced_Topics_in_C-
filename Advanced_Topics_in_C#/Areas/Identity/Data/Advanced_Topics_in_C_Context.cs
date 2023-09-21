using Lab02.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Advanced_Topics_in_C_.Data;

public class Advanced_Topics_in_C_Context : IdentityDbContext<IdentityUser>
{
    public Advanced_Topics_in_C_Context(DbContextOptions<Advanced_Topics_in_C_Context> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
    public DbSet<Account> Accounts { get; set; } = default!;
}
