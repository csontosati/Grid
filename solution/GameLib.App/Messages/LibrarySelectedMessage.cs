using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.App.Messages
{
    public class LibrarySelectedMessage(Guid libraryId)
    {
        public Guid LibraryId { get; } = libraryId;
    }
}
