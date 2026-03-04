using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.DAL.Entities
{
    public record TimerEntity : IEntity
    {
        public Guid Id { get; set; }
        public TimeSpan Time { get; set; }
    }
}
