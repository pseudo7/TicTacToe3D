using TicTacToe3D.Services;
using UnityEngine;

namespace TicTacToe3D.Shapes
{
    public class CrossShape : ShapeBase
    {
        protected override void SetupShape() =>
            ShapeRenderer = GetComponent<Renderer>();

        protected override void MoveToPosition(int segmentIndex) =>
            CurrentSegmentIndex = segmentIndex;

        protected internal override void SetMaterial(Material material) =>
            ShapeRenderer.material = material;

        protected internal override void SetMaterial(bool isFadedMat) =>
            SetMaterial(isFadedMat
                ? Bootstrap.BootstrapInstance.GetService<ResourcesService>().DarkWoodMatFaded
                : Bootstrap.BootstrapInstance.GetService<ResourcesService>().DarkWoodMat);
    }
}