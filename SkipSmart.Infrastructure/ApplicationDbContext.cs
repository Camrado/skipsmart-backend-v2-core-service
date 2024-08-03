using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SkipSmart.Application.Exceptions;
using SkipSmart.Domain.Abstractions;

namespace SkipSmart.Infrastructure;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork {
    private readonly IPublisher _publisher;
    
    public ApplicationDbContext(DbContextOptions options, IPublisher publisher) : base(options) {
        _publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        try {
            var result = await base.SaveChangesAsync(cancellationToken);

            await PublishDomainEventsAsync();

            return result;
        }
        catch (DbUpdateConcurrencyException ex) {
            throw new ConcurrencyException("Concurrency exception occurred", ex);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException postgresEx &&
                                           postgresEx.SqlState == "23505") {
            throw new DuplicateEmailException("Email is already taken", ex);
        }
        
    }

    private async Task PublishDomainEventsAsync() {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity => {
                var domainEvents = entity.GetDomainEvents();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (var domainEvent in domainEvents) {
            await _publisher.Publish(domainEvent);
        }
    }
}