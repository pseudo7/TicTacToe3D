using System.Collections.Generic;
using TicTacToe3D.Services;
using UnityEngine;

namespace TicTacToe3D.Pillars
{
    [System.Serializable]
    public abstract class PillarBase : MonoBehaviour
    {
        internal List<PillarSegment> PillarSegments { get; set; }
        protected internal int PillarIndex { get; set; }
        protected internal int PillarRowIndex { get; set; }
        protected internal int PillarColumnIndex { get; set; }
        protected abstract Renderer PillarRenderer { get; set; }
        protected internal abstract Vector3 SegmentSpawnPosition { get; }
        protected internal abstract Quaternion SegmentSpawnRotation { get; }

        protected abstract void SetupPillar();

        protected internal abstract void SetMaterial(Material material);
        protected internal abstract void SetSegmentMaterial(bool isFadedMat);

        protected virtual void Awake()
        {
        }

        protected virtual void Start() =>
            SetupPillar();

        private void OnMouseUpAsButton() =>
            OnPillarClicked();

        private void OnMouseDown() =>
            OnPillarHighlighted();

        private void OnMouseUp() =>
            OnPillarUnHighlighted();

        protected abstract void OnPillarClicked();

        protected virtual void OnPillarHighlighted() =>
            Bootstrap.BootstrapInstance.GetService<GameManagementService>()
                .CurrentGame.HighLightPillar(PillarIndex);

        protected virtual void OnPillarUnHighlighted() =>
            Bootstrap.BootstrapInstance.GetService<GameManagementService>()
                .CurrentGame.UnHighLightPillar(PillarIndex);
    }
}