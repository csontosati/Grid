using System;
using System.Collections.Generic;
using System.Text;
using GameLib.DAL.Entities;
namespace GameLib.DAL.Mappers
{
    public class UserEntityMapper : IEntityMapper<UserEntity>
    {
        public void MapToExistingEntity(UserEntity target, UserEntity source)
        {
            target.UserName = source.UserName;
            target.Email = source.Email;
            target.FirstName = source.FirstName;
            target.LastName = source.LastName;
        }
    }
}
