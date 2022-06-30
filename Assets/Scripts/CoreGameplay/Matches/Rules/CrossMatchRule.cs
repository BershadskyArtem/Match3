using CoreGameplay.Base;

namespace CoreGameplay.Matches.Rules
{
    public class CrossMatchRule : IMatchRule
    {

        private readonly IMatchRule hr;
        private readonly IMatchRule vr;

        public CrossMatchRule()
        {
            hr = new HorizontalMatchRule();
            vr = new VerticalMatchRule();
        }
        
        public bool TryGetMatchAtPoint(NodeObject[,] board, int xPos, int yPos, ref Match match)
        {
            var mh = match.CloneWithoutPositions();
            var mv = match.CloneWithoutPositions();

            hr.TryGetMatchAtPoint(board, xPos, yPos, ref mh);
            vr.TryGetMatchAtPoint(board, xPos, yPos, ref mv);

            match.AddRangePosition(mh.Positions);
            match.AddRangePosition(mv.Positions);

            return mh.Rank > 2 && mv.Rank > 2;
        }
    }
}