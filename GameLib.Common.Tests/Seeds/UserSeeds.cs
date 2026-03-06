using GameLib.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace GameLib.Common.Tests.Seeds;

public static class UserSeeds
{
    public static readonly UserEntity EmptyUser = new()
    {
        Id = default,
        UserName = default!,
        Email = default!
    };

    public static readonly UserEntity UserEntity = new()
    {
        Id = Guid.Parse("99158128-93DC-4797-BE90-C197730FC5E9"),
        UserName = "GamerOne",
        Email = "gamer@test.com",
        FirstName = "John",
        LastName = "Pork"
    };

    public static readonly UserEntity UserEntityUpdate = UserEntity with
    {
        Id = Guid.Parse("654E6B6D-4631-4380-8886-96CDEEAABE23"),
        UserName = "UpdatedUser"
    };

    public static readonly UserEntity UserEntityDelete = UserEntity with
    {
        Id = Guid.Parse("C78688EF-0690-49A3-90B6-8ED4582DDDCE"),
        UserName = "DeleteUser"
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