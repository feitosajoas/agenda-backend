using agenda.Models;
using Microsoft.EntityFrameworkCore;

namespace agenda.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<User> Users { get; set; }
}
