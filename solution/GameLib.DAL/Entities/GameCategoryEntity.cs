using System;
using System.Collections.Generic;
using System.Text;
using GameLib.DAL.Enums;

namespace GameLib.DAL.Entities
{
    internal record GameCategoryEntity : IEntity
    {
        public Guid Id { get; set; }
        public GameCategory TypeCategory { get; set; }

    }
}
