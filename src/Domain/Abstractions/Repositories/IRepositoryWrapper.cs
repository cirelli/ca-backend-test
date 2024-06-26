﻿namespace Domain.Abstractions.Repositories;

public interface IRepositoryWrapper
{
    ICustomerRepository Customer { get; }
    IProductRepository Product { get; }

    Task OpenTransactionAsync(CancellationToken cancellationToken);

    Task CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);

    Task SaveAsync(CancellationToken cancellationToken);
}
