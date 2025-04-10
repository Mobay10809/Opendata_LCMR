using Microsoft.EntityFrameworkCore;
using OpendataApi_LCMR.Models;

public class OpendataApiDbContext : DbContext
{

    public DbSet<Revenue>? Revenues { get; set; }
    public OpendataApiDbContext(DbContextOptions<OpendataApiDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Revenue>().HasNoKey();

    }

}
