using System;

namespace CoEvents
{
    public static class Extensions_Condition
    {
        public class ConditionMotor
        {
            public static ConditionMotor Create()
            {
                if (CoEvent.Pool == null) return new ConditionMotor();
                return (ConditionMotor)CoEvent.Pool.Allocate(typeof(ConditionMotor));
            }
            public static void Recycle(ConditionMotor motor)
            {
                motor.action = null;
                motor.removeWhen = null;
                motor.invokeWhen = null;
                if (CoEvent.Pool == null) return;
                CoEvent.Pool.Recycle(typeof(ConditionMotor), motor);
            }

            private void Update(float x)
            {
                if (invokeWhen == null ? true : invokeWhen())
                {
                    action?.Invoke();
                }
                if (removeWhen == null ? false : removeWhen())
                {
                    CoEvent.Instance.Operator<IUpdate>().UnSubscribe(Update);
                    Recycle(this);
                }
            }
            internal ConditionMotor SetUpdateInvoke(Action action)
            {
                this.action = action;
                CoEvent.Instance.Operator<IUpdate>().Subscribe(Update);
                return this;
            }
            private void LateUpdate()
            {
                if (invokeWhen == null ? true : invokeWhen())
                {
                    action?.Invoke();
                }
                if (removeWhen == null ? false : removeWhen())
                {
                    CoEvent.Instance.Operator<ILateUpdate>().UnSubscribe(LateUpdate);
                    Recycle(this);
                }
            }

            internal ConditionMotor SetLateUpdateInvoke(Action action)
            {
                this.action = action;
                CoEvent.Instance.Operator<ILateUpdate>().Subscribe(LateUpdate);
                return this;
            }

            private void FixedUpdate()
            {
                if (invokeWhen == null ? true : invokeWhen())
                {
                    action?.Invoke();
                }
                if (removeWhen == null ? false : removeWhen())
                {
                    CoEvent.Instance.Operator<ILateUpdate>().UnSubscribe(FixedUpdate);
                    Recycle(this);
                }
            }

            internal ConditionMotor SetFixedUpdateInvoke(Action action)
            {
                this.action = action;
                CoEvent.Instance.Operator<ILateUpdate>().Subscribe(FixedUpdate);
                return this;
            }




            private Action action = null;
            private Func<bool> removeWhen = null;
            private Func<bool> invokeWhen = null;





            public ConditionMotor RemoveWhen(Func<bool> rm)
            {
                removeWhen = rm;
                return this;
            }
            public ConditionMotor Condition(Func<bool> im)
            {
                invokeWhen = im;
                return this;
            }
        }
        public static ConditionMotor Invoke(this ICoVarOperator<IUpdate> oper, Action action)
        {
            var motor = ConditionMotor.Create();
            return motor.SetUpdateInvoke(action);
        }
        public static ConditionMotor Invoke(this ICoVarOperator<ILateUpdate> oper, Action action)
        {
            var motor = ConditionMotor.Create();
            return motor.SetUpdateInvoke(action);
        }
        public static ConditionMotor Invoke(this ICoVarOperator<IFixedUpdate> oper, Action action)
        {
            var motor = ConditionMotor.Create();
            return motor.SetUpdateInvoke(action);
        }
    }
}
