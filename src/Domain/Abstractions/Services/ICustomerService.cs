using Domain.Dtos;
using Domain.ServiceResults;

using DTO = Domain.Dtos.CustomerDTO;
using TEntity = Domain.Entities.Customer;

namespace Domain.Abstractions.Services;

public interface ICustomerService
{
    Task<ServiceResult<TModel>> GetByIdAsync<TModel>(Guid id, CancellationToken cancellationToken);

    Task<ServiceResult<List<TModel>>> GetAllAsync<TModel>(Pagination pagination, CancellationToken cancellationToken);

    Task<ServiceResult<TEntity>> CreateAsync(DTO dto, CancellationToken cancellationToken);

    Task<ServiceResult> UpdateAsync(Guid id, DTO dto, CancellationToken cancellationToken);

    Task<ServiceResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
