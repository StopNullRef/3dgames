using System;

namespace Project.Util
{
    public interface IPoolableObject
    {
        bool CanRecycle { get; set; }
        void PoolInit(); 
    }
}
