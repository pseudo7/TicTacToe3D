using TicTacToe3D.Pillars;
using TicTacToe3D.StateMachine;
using TicTacToe3D.Utilities;
using UnityEngine;

namespace TicTacToe3D.Games
{
    public abstract class GameBase : MonoBehaviour
    {
        protected internal abstract GameEnums.ShapeType[,,] GameMatrix { get; set; }
        protected internal abstract PillarBase[] Pillars { get; set; }
        protected abstract IStateMachine GameStateMachine { get; set; }
        protected internal abstract SwipeRotator CurrentSwipeRotator { get; set; }
        protected abstract int Dimension { get; set; }
        protected abstract void SetupGame();
        protected internal abstract void HighLightPillar(int pillarIndex);
        protected internal abstract void UnHighLightPillar(int pillarIndex);
        protected internal abstract void AddShape(int pillarIndex);
        protected abstract bool CheckForFinish();

        protected virtual void Awake() =>
            SetupGame();
    }
}