﻿using DTO = Domain.Dtos.ProductDTO;
using IRepository = Domain.Abstractions.Repositories.IProductRepository;
using TEntity = Domain.Entities.Product;

namespace Services;

public class ProductService(IRepositoryWrapper RepositoryWrapper,
                            IMapper mapper,
                            IValidator<DTO> dtoValidator)
    : BaseService,
    IProductService
{
    protected IRepository Repository => RepositoryWrapper.Product;

    public async Task<ServiceResult<TModel>> GetByIdAsync<TModel>(Guid id,
                                                                  CancellationToken cancellationToken)
    {
        TModel? value = await Repository.GetByIdAsync<TModel>(id, cancellationToken);

        if (value is null)
        {
            return NotFound();
        }

        return Success(value);
    }

    public async Task<ServiceResult<List<TModel>>> GetAllAsync<TModel>(Pagination pagination,
                                                                       CancellationToken cancellationToken)
    {
        pagination ??= new();

        List<TModel> values = await Repository.GetAllAsync<TModel>(pagination, cancellationToken);

        return Success(values);
    }

    public async Task<ServiceResult<TEntity>> CreateAsync(DTO dto,
                                                          CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await dtoValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationError(validationResult);
        }

        TEntity entity = mapper.Map<TEntity>(dto);
        Repository.Create(entity);
        await RepositoryWrapper.SaveAsync(cancellationToken);

        return Success(entity);
    }

    public async Task<ServiceResult> UpdateAsync(Guid id,
                                                 DTO dto,
                                                 CancellationToken cancellationToken)
    {
        TEntity? entity = await Repository.GetByIdAsync<TEntity>(id, cancellationToken);
        if (entity is null)
        {
            return NotFound();
        }

        ValidationResult validationResult = await dtoValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationError(validationResult);
        }

        mapper.Map(dto, entity);
        entity.UpdatedAt = DateTime.UtcNow;
        Repository.Update(entity);
        await RepositoryWrapper.SaveAsync(cancellationToken);

        return Success();
    }

    public async Task<ServiceResult> DeleteAsync(Guid id,
                                                 CancellationToken cancellationToken)
    {
        if (!(await Repository.ExistsAsync(id, cancellationToken)))
        {
            return NotFound();
        }

        await Repository.DeleteAsync(id, cancellationToken);

        return Success();
    }
}