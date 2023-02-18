namespace Lost.Utils.StateMachine
{
    public interface IStateWithPayload<in T>
        where T: class 
    {
        void Enter(T payload);
    }
}