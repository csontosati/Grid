using GameLib.DAL.Entities;
using GameLib.BL.Facades.Interfaces;
using GameLib.BL.Models;

namespace GameLib.BL.Facades;

public interface IUserFacade : IFacade<UserEntity, UserListModel, UserDetailModel>
{
}