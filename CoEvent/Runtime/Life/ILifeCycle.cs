namespace CoEvent
{
    public interface IUpdate : ISendEvent<float> { };

    public interface IFixedUpdate : ISendEvent { }

    public interface ILateUpdate : ISendEvent { }

}
