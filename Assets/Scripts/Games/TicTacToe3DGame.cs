using TicTacToe3D.Pillars;
using TicTacToe3D.Shapes;

namespace TicTacToe3D.Games
{
    public class TicTacToe3DGame : GameBase
    {
        protected internal override bool[,,] GameMatrix { get; set; }
        protected internal override PillarBase[] Pillars { get; set; }

        protected override void SetupGame()
        {
            GameMatrix = new bool[3, 3, 3];
            Pillars = new PillarBase[9];
        }

        protected internal override void AddShape(ShapeBase shape, int pillarIndex)
        {
            var pillar = Pillars[pillarIndex];
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