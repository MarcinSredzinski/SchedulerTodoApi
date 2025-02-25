using Business.Library.Models;
using Microsoft.EntityFrameworkCore;

namespace SchedulerTodo.DB;

public class SqlServerDbContext(DbContextOptions<SqlServerDbContext> optionsBuilder) : DbContext(optionsBuilder)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<ItemToDo>().HasQueryFilter(i => !i.IsDeleted);

        modelBuilder.Entity<Appointment>().HasIndex(r => r.IsDeleted).HasFilter("IsDeleted = 0");
        modelBuilder.Entity<ApiKey>().HasData(new ApiKey(){ Id = 1, Key = "k8FZGeZg#I#6b1SwblyU^49TeZLtHLP!y!sB2boP*djNMFosfd"});
    }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<ItemToDo> ItemsToDo { get; set; }
    public DbSet<ApiKey> ApiKeys { get; set; }
}