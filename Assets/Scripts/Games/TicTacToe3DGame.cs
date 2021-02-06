using System;
using TicTacToe3D.Pillars;
using TicTacToe3D.Services;
using TicTacToe3D.Shapes;
using UnityEngine;

namespace TicTacToe3D.Games
{
    public class TicTacToe3DGame : GameBase
    {
        protected internal override bool[,,] GameMatrix { get; set; }
        protected internal override PillarBase[] Pillars { get; set; }

        private int _currentShapeCount;
        private int _currentMaterialCount;

        protected override void SetupGame()
        {
            GameMatrix = new bool[3, 3, 3];
            Pillars = new PillarBase[9];
        }

        protected internal override void HighLightPillar(int pillarIndex)
        {
            var count = Pillars.Length;
            while (count-- > 0)
                Pillars[count]
                    .SetMaterial(Bootstrap.BootstrapInstance.GetService<ResourcesService>().LightWoodMatFaded);
            Pillars[pillarIndex].SetMaterial(Bootstrap.BootstrapInstance.GetService<ResourcesService>().LightWoodMat);
        }

        protected internal override void UnHighLightPillar(int pillarIndex)
        {
            var count = Pillars.Length;
            while (count-- > 0)
                Pillars[count].SetMaterial(Bootstrap.BootstrapInstance.GetService<ResourcesService>().LightWoodMat);
        }

        protected internal override void AddShape(int pillarIndex)
        {
            var pillar = Pillars[pillarIndex];
            var targetSegment = pillar.PillarSegments
                .Find(x => !x.CurrentShape);
            if (!targetSegment)
            {
                Debug.LogError($"Segments are filled for Pillar: {pillarIndex}");
                return;
            }

            var shapePrefab = (_currentShapeCount++ & 1) != 0
                ? Bootstrap.BootstrapInstance.GetService<ResourcesService>().CrossPrefab
                : Bootstrap.BootstrapInstance.GetService<ResourcesService>().SpherePrefab;
            var shape = Instantiate(shapePrefab, pillar.SegmentSpawnPosition, pillar.SegmentSpawnRotation);
            shape.transform.SetParent(pillar.transform, true);
            shape.SetMaterial((_currentMaterialCount++ & 1) != 0
                ? Bootstrap.BootstrapInstance.GetService<ResourcesService>().DarkWoodMat
                : Bootstrap.BootstrapInstance.GetService<ResourcesService>().LightWoodMat);

            targetSegment.CurrentShape = shape;
            targetSegment.SetSegment();
        }
    }
}