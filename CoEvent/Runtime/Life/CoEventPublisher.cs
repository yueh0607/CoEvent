using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoEvent
{
    public class CoEventPublisher : MonoBehaviour
    {

        void FixedUpdate()
        {
            this.Operator<IFixedUpdate>().Send();
        }
        void Update()
        {
            this.Operator<IUpdate>().Send(Time.deltaTime);
        }

        void LateUpdate()
        {
            this.Operator<ILateUpdate>().Send();
        }
    }
}
