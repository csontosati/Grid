using System.Collections.ObjectModel;
using GameLib.BL.Mappers.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;

namespace GameLib.BL.Mappers;

public class UserModelMapper(
    IModelMapper<LibraryEntity, LibraryListModel, LibraryDetailModel> libraryMapper)
    : ModelMapperBase<UserEntity, UserListModel, UserDetailModel>
{
    public override UserListModel MapToListModel(UserEntity? entity)
        => entity is null
            ? UserListModel.Empty
            : new UserListModel
            {
                Id = entity.Id,
                UserName = entity.UserName

            };

    public override UserDetailModel MapToDetailModel(UserEntity? entity)
        => entity is null
            ? UserDetailModel.Empty
            : new UserDetailModel
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,

                Libraries = new ObservableCollection<LibraryListModel>(
                    entity.Libraries.Select(libraryMapper.MapToListModel)
                )
            };

    public override UserEntity MapToEntity(UserDetailModel model)
        => new UserEntity
        {
            Id = model.Id,
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };
}