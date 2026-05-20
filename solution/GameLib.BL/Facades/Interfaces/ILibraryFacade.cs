using GameLib.BL.Mappers.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;
using GameLib.DAL.Mappers;
using GameLib.DAL.UnitOfWork;

namespace GameLib.BL.Facades.Interfaces;

public interface ILibraryFacade : IFacade<LibraryEntity, LibraryListModel, LibraryDetailModel>
{
    Task AddGameAsync(Guid libraryId, Guid gameId);
    Task RemoveGameAsync(Guid libraryId, Guid gameId);
    Task<IList<LibraryListModel>> GetByUserAsync(Guid userId);
}