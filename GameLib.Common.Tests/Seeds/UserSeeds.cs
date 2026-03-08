using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLib.Common.Tests.Seeds;

public static class UserSeeds
{
    public static UserEntity EmptyUser => new()
    {
        Id = default,
        UserName = default!,
        Email = default!
    };

    public static UserEntity UserEntity => new()
    {
        Id = Guid.Parse("99158128-93DC-4797-BE90-C197730FC5E9"),
        UserName = "GamerOne",
        Email = "gamer@test.com",
        FirstName = "John",
        LastName = "Pork",
        Libraries = new List<LibraryEntity>()
    };

    public static UserEntity UserEntityUpdate => new()
    {
        Id = Guid.Parse("654E6B6D-4631-4380-8886-96CDEEAABE23"),
        UserName = "UpdatedUser",
        Email = "gamer@test.com",
        FirstName = "John",
        LastName = "Pork",
        Libraries = new List<LibraryEntity>()
    };

    public static UserEntity UserEntityDelete => new()
    {
        Id = Guid.Parse("C78688EF-0690-49A3-90B6-8ED4582DDDCE"),
        UserName = "DeleteUser",
        Email = "gamer@test.com",
        FirstName = "John",
        LastName = "Pork",
        Libraries = new List<LibraryEntity>()
    };

    public static DbContext SeedUsers(this DbContext dbx)
    {
        dbx.Set<UserEntity>().AddRange(
            UserEntity,
            UserEntityUpdate,
            UserEntityDelete
        );
        return dbx;
    }
}