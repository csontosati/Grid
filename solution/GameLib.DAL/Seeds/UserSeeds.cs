using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameLib.DAL.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity User1 = new()
    {
        Id = Guid.Parse("99158128-93DC-4797-BE90-C197730FC5E9"),
        UserName = "GamerOne",
        Email = "gamer1@test.com",
        FirstName = "John",
        LastName = "Pork",
        Libraries = new List<LibraryEntity>()
    };

    public static readonly UserEntity User2 = new()
    {
        Id = Guid.Parse("A2B2C2D2-AAAA-4BBB-8CCC-1234567890A1"),
        UserName = "PlayerTwo",
        Email = "player2@test.com",
        FirstName = "Jane",
        LastName = "Doe",
        Libraries = new List<LibraryEntity>()
    };

    public static readonly UserEntity User3 = new()
    {
        Id = Guid.Parse("B3C3D3E3-BBBB-4CCC-8DDD-1234567890B2"),
        UserName = "ThirdGuy",
        Email = "thirdguy@test.com",
        FirstName = "Alice",
        LastName = "Smith",
        Libraries = new List<LibraryEntity>()
    };

    public static DbContext SeedUsers(this DbContext dbx)
    {
        dbx.Set<UserEntity>().AddRange(User1, User2, User3);
        return dbx;
    }
}
