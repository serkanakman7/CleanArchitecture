using App.Application.Contracts.Persistence;

namespace App.Persistence;

public class UnitOfWork : IUnitOfWork
{
    protected readonly AppDbContext Context;
    public UnitOfWork(AppDbContext context)
    {
        Context = context;
    }

    public Task<int> SaveChangesAsync()
    {
        return Context.SaveChangesAsync();
    }
}
