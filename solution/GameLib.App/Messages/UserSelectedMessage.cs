using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.App.Messages
{
    public class UserSelectedMessage(Guid userId)
    {
        public Guid UserId { get; } = userId;
    }
}
