using CommunityToolkit.Mvvm.Messaging.Messages;
using GameLib.BL.Models;

namespace GameLib.App.Messages;

public class UserAddedMessage : ValueChangedMessage<UserListModel>
{
    public UserAddedMessage(UserListModel newUser) : base(newUser)
    {
    }
}