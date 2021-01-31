namespace TicTacToe3D.Pillars
{
    public class ThreeSegmentPillar : PillarBase
    {
        protected override void SetupPillar()
        {
            PillarIndex = transform.GetSiblingIndex();
            PillarRowIndex = PillarIndex / 3;
            PillarColumnIndex = PillarIndex % 3;
            
        }
    }
}