namespace SharedLibrary;

public interface IUnitOfWork:IDisposable
{
    Task<int> SaveChangesAsync(); // Save changes
    Task BeginTransactionAsync(); // Begin a transaction
    Task CommitTransactionAsync(); // Commit the transaction
    Task RollbackTransactionAsync(); // Rollback the transaction
}