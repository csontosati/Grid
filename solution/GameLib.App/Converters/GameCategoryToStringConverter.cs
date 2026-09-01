using CommunityToolkit.Maui.Converters;
using GameLib.DAL.Enums;
using System.Globalization;
using GameLib.App.Resources.Texts;

namespace GameLib.App.Converters;


public class GameCategoryToStringConverter : BaseConverterOneWay<GameCategory, string>
{
    public override string ConvertFrom(GameCategory value, CultureInfo? culture)
        => GameCategoryTexts.ResourceManager.GetString(value.ToString(), culture)
           ?? GameCategoryTexts.None;

    public override string DefaultConvertReturnValue { get; set; } = GameCategoryTexts.None;
}