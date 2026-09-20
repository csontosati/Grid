using GameLib.App.Services;
using System.Linq;

namespace GameLib.App.Services;

public class AlertService : IAlertService
{
    public async Task DisplayAsync(string title, string message)
    {
        var page = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page is null)
        {
            return;
        }

        await page.DisplayAlertAsync(title, message, "OK");
    }

    public async Task<bool> DisplayConfirmAsync(string title, string message, string accept, string cancel)
    {
        var page = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (page is null)
        {
            return false;
        }

        return await page.DisplayAlert(title, message, accept, cancel);
    }
}