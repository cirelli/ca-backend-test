using Microsoft.EntityFrameworkCore.Storage;

namespace Infraestructure.Repositories;

public class RepositoryWrapper(DataContext DataContext,
                               IMapper Mapper)
    : IRepositoryWrapper
{
    private IDbContextTransaction? transaction;

    private ICustomerRepository? customerRepository;
    public ICustomerRepository Customer
        => customerRepository ??= new CustomerRepository(DataContext, Mapper);

    private IProductRepository? productRepository;
    public IProductRepository Product
        => productRepository ??= new ProductRepository(DataContext, Mapper);

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (transaction is not null)
        {
            await transaction.CommitAsync(cancellationToken);
        }
    }

    public async Task OpenTransactionAsync(CancellationToken cancellationToken)
        => transaction = DataContext.Database.CurrentTransaction
            ?? await DataContext.Database.BeginTransactionAsync(cancellationToken);

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (transaction is not null)
        {
            await transaction.RollbackAsync(cancellationToken);
        }
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
        => await DataContext.SaveChangesAsync(cancellationToken);
}
