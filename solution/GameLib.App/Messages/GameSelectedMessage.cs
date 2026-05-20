using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.App.Messages
{
    public class GameSelectedMessage(Guid gameid)
    {
        public Guid GameId { get; } = gameid;

    }
}
