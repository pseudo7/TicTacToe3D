using System;

namespace TicTacToe3D.StateMachine
{
    public interface IState
    {
        bool IsStateCompleted { get; set; }
        Action<IState> StateAction { get; set; }
    }
}