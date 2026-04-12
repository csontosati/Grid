using System.Collections.ObjectModel;
using GameLib.BL.Models;
using GameLib.DAL.Entities;

namespace GameLib.BL.Mappers;

public class GameModelMapper
    : ModelMapperBase<GameEntity, GameListModel, GameDetailModel>
{
    public override GameListModel MapToListModel(GameEntity? entity)
        => entity is null
            ? GameListModel.Empty
            : new GameListModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ImageUrl = entity.ImageUrl
            };

    public override GameDetailModel MapToDetailModel(GameEntity? entity)
        => entity is null
            ? GameDetailModel.Empty : new GameDetailModel
            {
                Id = entity.Id,
                StudioId = entity.StudioId,
                StudioName = entity.Studio?.Name,

                Name = entity.Name,
                Description = entity.Description,
                Age = entity.Age,
                ImageUrl = entity.ImageUrl,

                CategoryNames = new ObservableCollection<string>(
                    entity.Categories.Select(c => c.Category.ToString())
                ),

                TimePlayed = entity.Timer
                    .Aggregate(TimeSpan.Zero, (sum, t) => sum + t.Time)
            };
    public override GameEntity MapToEntity(GameDetailModel model)
        => new()
        {
            Id = model.Id,
            StudioId = model.StudioId,
            Name = model.Name,
            Description = model.Description,
            Age = model.Age,
            ImageUrl = model.ImageUrl
        };
}