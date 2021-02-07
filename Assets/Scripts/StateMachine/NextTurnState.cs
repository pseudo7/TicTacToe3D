using System;

namespace TicTacToe3D.StateMachine
{
    public class NextTurnState : IState
    {
        public bool IsStateCompleted { get; set; }

        public Action<IState> StateAction { get; set; }

        public NextTurnState(Action<IState> newStateAction) => StateAction = newStateAction;

        public NextTurnState()
        {
        }
    }
}