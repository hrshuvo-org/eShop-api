using Framework.App.Data;
using Framework.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Framework.Core.Data;

public class DataContext : IdentityDbContext<AppUser, AppRole, long, IdentityUserClaim<long>,
    AppUserRole, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        RegisterModelBuilder(builder);
    }

    private void RegisterModelBuilder(ModelBuilder builder)
    {
        CoreModelBuilder.Build(builder);
        AppModelBuilder.Build(builder);
    }
}