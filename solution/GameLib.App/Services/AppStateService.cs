using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.App.Services;

public class AppState
{
    public Guid CurrentUserId { get; set; }
    public Guid CurrentLibraryId { get; set; }
}
