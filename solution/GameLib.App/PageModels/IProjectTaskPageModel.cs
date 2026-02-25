using CommunityToolkit.Mvvm.Input;
using GameLib.App.Models;

namespace GameLib.App.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}