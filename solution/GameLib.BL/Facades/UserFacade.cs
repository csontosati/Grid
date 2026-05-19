using GameLib.BL.Mappers;
using GameLib.BL.Models;
using GameLib.BL.Facades.Interfaces;
using GameLib.DAL.Entities;
using GameLib.DAL.Mappers;
using GameLib.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace GameLib.BL.Facades;

public class UserFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    UserModelMapper modelMapper)
    : BaseFacade<
        UserEntity,
        UserListModel,
        UserDetailModel,
        UserEntityMapper>(
        unitOfWorkFactory,
        modelMapper)
{
    protected override ICollection<string> IncludesNavigationPathDetail =>
        new[]
        {
            nameof(UserEntity.Libraries)
        };

    public class Filter
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? OrderBy { get; set; }
    }

    protected override IQueryable<UserEntity> ApplyFilter(
        IQueryable<UserEntity> query,
        object? filter)
    {
        if (filter is not Filter f)
            return query;

        if (!string.IsNullOrWhiteSpace(f.UserName))
            query = query.Where(u => u.UserName.Contains(f.UserName));

        if (!string.IsNullOrWhiteSpace(f.Email))
            query = query.Where(u => u.Email.Contains(f.Email));

        return query;
    }

    protected override IQueryable<UserEntity> ApplyOrder(
        IQueryable<UserEntity> query,
        object? filter)
    {
        if (filter is not Filter f || string.IsNullOrWhiteSpace(f.OrderBy))
            return query;

        return f.OrderBy switch
        {
            "username" => query.OrderBy(u => u.UserName),
            "username_desc" => query.OrderByDescending(u => u.UserName),
            "email" => query.OrderBy(u => u.Email),
            _ => query
        };
    }

    public async Task<UserDetailModel?> GetByUsernameAsync(string username)
    {
        await using var uow = UnitOfWorkFactory.Create();

        var entity = await uow.GetRepository<UserEntity, UserEntityMapper>()
            .Get()
            .Include(u => u.Libraries)
            .SingleOrDefaultAsync(u => u.UserName == username);
        
        return entity is null ? null : ModelMapper.MapToDetailModel(entity);
    }
}