using UnityEngine;

namespace CoEvents
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
