using BackgroundServiceMath.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackgroundServiceMath.Data;

public class BackgroundServiceContext(DbContextOptions<BackgroundServiceContext> options) : IdentityDbContext(options)
{
    public DbSet<Player> Player { get; set; } = default!;
}