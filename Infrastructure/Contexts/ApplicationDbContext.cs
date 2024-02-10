using Domain.EmailTemplates;
using Domain.EventNotifications;
using Domain.Reminders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
    : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(ApplicationDbContext).Assembly;
        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Reminder> Reminders { get; set; }
    public DbSet<EventNotification> EventNotifications { get; set; }
    public DbSet<EmailTemplate> EmailTemplates { get; set; }
}