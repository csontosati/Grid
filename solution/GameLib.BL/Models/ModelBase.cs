using CommunityToolkit.Mvvm.ComponentModel;

namespace GameLib.BL.Models;

public abstract class ModelBase : ObservableObject
{
    public Guid Id { get; set; }
}