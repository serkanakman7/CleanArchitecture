using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories;

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
