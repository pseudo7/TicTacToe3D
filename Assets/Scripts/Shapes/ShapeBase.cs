using UnityEngine;

namespace TicTacToe3D.Shapes
{
    public abstract class ShapeBase : MonoBehaviour
    {
        internal protected int CurrentSegmentIndex { get; set; }
        protected Renderer ShapeRenderer { get; set; }
        protected void Awake() => SetupShape();
        protected abstract void SetupShape();
        protected abstract void MoveToPosition(int segmentIndex);
        protected internal abstract void SetMaterial(Material material);
    }
}