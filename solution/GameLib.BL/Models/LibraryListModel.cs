using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.BL.Models;

public partial class LibraryListModel: ModelBase
{
    public required string Name { get; set; }

    public static LibraryListModel Empty => new() {
        Id = Guid.Empty,
        Name = string.Empty
    };
}