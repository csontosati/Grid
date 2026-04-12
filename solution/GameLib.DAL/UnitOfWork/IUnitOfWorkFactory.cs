using System;
using System.Collections.Generic;
using System.Text;

namespace GameLib.DAL.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}
