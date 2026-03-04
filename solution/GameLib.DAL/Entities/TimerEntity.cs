using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.DAL.Entities
{
    internal record TimerEntity : IEntity
    {
        public Guid Id { get; set; }
        public TimeSpan Time { get; set; }
    }
}
