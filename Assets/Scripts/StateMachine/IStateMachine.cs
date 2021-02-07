namespace TicTacToe3D.StateMachine
{
    public interface IStateMachine
    {
        void Initialisation(IState initialState);
        void UpdateState(IState nextState);
        void Finalisation();
    }
}