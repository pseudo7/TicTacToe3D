using TicTacToe3D.Pillars;
using TicTacToe3D.Services;
using TicTacToe3D.Shapes;

namespace TicTacToe3D.Games
{
    public class TicTacToe3DGame : GameBase
    {
        protected internal override bool[,,] GameMatrix { get; set; }
        protected internal override PillarBase[] Pillars { get; set; }

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
            var shape = Instantiate(Bootstrap.BootstrapInstance.GetService<ResourcesService>().CrossPrefab,
                pillar.SegmentSpawnPosition, pillar.SegmentSpawnRotation);
            shape.SetMaterial(((_currentMaterialCount++) & 1) != 0
                ? Bootstrap.BootstrapInstance.GetService<ResourcesService>().DarkWoodMat
                : Bootstrap.BootstrapInstance.GetService<ResourcesService>().LightWoodMat);


            var rowIndex = pillar.PillarRowIndex;
            var columnIndex = pillar.PillarColumnIndex;

            var segmentIndex = 0;
            for (; segmentIndex < 3; segmentIndex++)
                if (!GameMatrix[rowIndex, columnIndex, segmentIndex])
                    break;

            //TODO: Get a better from Pillar class only to add shape directly instead of doing these calculations
        }
    }
}