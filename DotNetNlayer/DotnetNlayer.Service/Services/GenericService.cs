using System.Linq.Expressions;
using DotNetNlayer.Core.Repositories;
using DotNetNlayer.Core.Services;
using SharedLibrary;

namespace DotnetNlayer.Service.Services;

public class GenericService<T> : IGenericService<T> where T :class
{
    private readonly IGenericRepository<T?> _repository;
    private readonly IUnitOfWork _unitOfWork;
    public GenericService(IGenericRepository<T?> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public IQueryable<T?> Where(Expression<Func<T?, bool>> expression)
    {
        var entities = _repository.Where(expression);
        return entities;
    }
    public Task Update(T? entity)
    {
        _repository.Update(entity);
        return Task.CompletedTask;
    }

    public Task Remove(T entity)
    {
        _repository.Remove(entity);
        return Task.CompletedTask;
    }


}