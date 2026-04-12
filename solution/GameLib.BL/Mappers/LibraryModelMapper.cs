using System.Collections.ObjectModel;
using GameLib.BL.Models;
using GameLib.DAL.Entities;

namespace GameLib.BL.Mappers;

public class LibraryModelMapper
    : ModelMapperBase<LibraryEntity, LibraryListModel, LibraryDetailModel>
{
    public override LibraryListModel MapToListModel(LibraryEntity? entity)
        => entity is null
            ? LibraryListModel.Empty
            : new LibraryListModel
            {
                Id = entity.Id,
                Name = entity.Name,
            };

    public override LibraryDetailModel MapToDetailModel(LibraryEntity? entity)
        => entity is null
            ? LibraryDetailModel.Empty
            : new LibraryDetailModel
            {
                Id = entity.Id,
                Name = entity.Name,
                UserId = entity.UserId

            };
    public override LibraryEntity MapToEntity(LibraryDetailModel model)
        => new()
        {
            Id = model.Id,
            Name = model.Name,
            UserId = model.UserId
        };
}