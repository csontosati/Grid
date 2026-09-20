namespace GameLib.App.Services;

public interface IAlertService
{
    Task DisplayAsync(string title, string message);
    Task<bool> DisplayConfirmAsync(string title, string message, string accept, string cancel);
}