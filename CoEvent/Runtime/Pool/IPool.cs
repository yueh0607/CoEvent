using System;
using System.Collections;
using System.Collections.Generic;

namespace CoEvents
{
    public interface IPool
    {
        public object Allocate(Type type);
        public void Recycle(Type type, object item);
    }

   
}
