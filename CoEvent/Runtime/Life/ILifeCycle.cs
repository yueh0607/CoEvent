using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoEvent
{
    public interface IUpdate : ISendEvent<float> { };

    public interface IFixedUpdate : ISendEvent { }

    public interface ILateUpdate : ISendEvent { }
    
}
