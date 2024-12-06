using PetHouse.Persistence.Interfaces;

namespace PetHouse.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly PetHouseDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(PetHouseDbContext context)
    {
        _context = context;
    }

    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T);
        if (!_repositories.ContainsKey(type))
        {
            var repositoryInstance = new GenericRepository<T>(_context);
            _repositories[type] = repositoryInstance;
        }

        return (IRepository<T>)_repositories[type];
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}