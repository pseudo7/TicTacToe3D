namespace TicTacToe3D.Shapes
{
    public class CrossShape : ShapeBase
    {
        protected override void SetupShape()
        {
        }

        protected override void MoveToPosition(int segmentIndex)
        {
            CurrentSegmentIndex = segmentIndex;
        }
    }
}