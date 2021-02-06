using TicTacToe3D.Services;
using UnityEngine;

namespace TicTacToe3D.Pillars
{
    public class ThreeSegmentPillar : PillarBase
    {
        protected override Renderer PillarRenderer { get; set; }
        protected internal override Vector3 SegmentSpawnPosition { get; set; }
        protected internal override Quaternion SegmentSpawnRotation { get; set; }

        protected override void SetupPillar()
        {
            PillarIndex = transform.GetSiblingIndex();
            PillarRowIndex = PillarIndex / 3;
            PillarColumnIndex = PillarIndex % 3;
            Bootstrap.BootstrapInstance.GetService<GameManagementService>()
                .CurrentGame.Pillars[PillarIndex] = this;
            PillarRenderer = GetComponent<Renderer>();
            SegmentSpawnPosition = transform.position + Vector3.up * 12;
            SegmentSpawnRotation = transform.rotation;
        }

        protected internal override void SetMaterial(Material material) =>
            PillarRenderer.material = material;

        protected override void OnPillarClicked()
        {
            Bootstrap.BootstrapInstance.GetService<GameManagementService>()
                .CurrentGame.AddShape(PillarIndex);
            Debug.Log($"Pillar Index: {PillarIndex}, Row: {PillarRowIndex}, Column: {PillarColumnIndex}");
        }
    }
}