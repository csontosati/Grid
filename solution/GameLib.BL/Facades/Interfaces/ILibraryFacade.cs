using GameLib.BL.Mappers.Interfaces;
using GameLib.BL.Models;
using GameLib.DAL.Entities;
using GameLib.DAL.Mappers;
using GameLib.DAL.UnitOfWork;

namespace GameLib.BL.Facades.Interfaces;

public interface ILibraryFacade
{
    public Task AddGameAsync(Guid libraryId, Guid gameId);


    public Task RemoveGameAsync(Guid libraryId, Guid gameId);
    public Task<IList<LibraryListModel>> GetByUserAsync(Guid userId);
}