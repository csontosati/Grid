using GameLib.BL.Mappers;
using GameLib.BL.Models;
using GameLib.BL.Facades.Interfaces;
using GameLib.DAL.Entities;
using GameLib.DAL.Enums;
using GameLib.DAL.Mappers;
using GameLib.DAL.UnitOfWork;
using System.Linq;

namespace GameLib.BL.Facades;

public class UserFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    UserModelMapper modelMapper)
    : BaseFacade<
        UserEntity,
        UserListModel,
        UserDetailModel,
        UserEntityMapper>(
            unitOfWorkFactory,
            modelMapper)
{
  
}