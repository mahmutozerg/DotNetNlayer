using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SharedLibrary;

namespace DotnetNlayer.Repository.Repositories;

public class UnitOfWork:IUnitOfWork
{
    private readonly DbContext _context;
    private IDbContextTransaction _transaction = null!;

    public UnitOfWork(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }


    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }


    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch
        {
            await _transaction.RollbackAsync();
            throw;
        }
        
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction.RollbackAsync();
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _transaction?.Dispose();
        _context.Dispose();    
    }
}