using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoEvent
{
    public interface IPool
    {
        public object Allocate(Type type);
        public void Recycle(Type type,object item);
    }
}
