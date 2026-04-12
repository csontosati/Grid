using System;
using System.Collections.Generic;
using System.Text;
using GameLib.DAL.Entities;
namespace GameLib.DAL.Mappers;

public class LibraryEntityMapper : IEntityMapper<LibraryEntity>
{
    public void MapToExistingEntity(LibraryEntity target, LibraryEntity source)
    {
        target.Name = source.Name;
        target.UserId = source.UserId;
    }
}
