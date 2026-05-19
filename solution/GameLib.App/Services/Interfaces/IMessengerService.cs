using CommunityToolkit.Mvvm.Messaging;

namespace GameLib.App.Services;

public interface IMessengerService
{
    IMessenger Messenger { get; }

    void Send<TMessage>(TMessage message)
        where TMessage : class;
}