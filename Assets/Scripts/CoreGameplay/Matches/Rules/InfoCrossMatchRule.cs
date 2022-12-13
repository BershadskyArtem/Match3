using CoreGameplay.Base;
using CoreGameplay.Kinds;
using CoreGameplay.Nodes;

namespace CoreGameplay.Matches.Rules
{
    public class InfoCrossMatchRule : IMatchRule<NodeInfo[,]>
    {
        private readonly IMatchRule<NodeInfo[,]> hr;
        private readonly IMatchRule<NodeInfo[,]> vr;

        public InfoCrossMatchRule()
        {
            hr = new InfoHorizontalMatchRule();
            vr = new InfoVerticalMatchRule();
        }
        
        public bool TryGetMatchAtPoint(NodeInfo[,] board, int xPos, int yPos, ref Match match)
        {
            var mh = match.CloneWithoutPositions();
            var mv = match.CloneWithoutPositions();

            hr.TryGetMatchAtPoint(board, xPos, yPos, ref mh);
            vr.TryGetMatchAtPoint(board, xPos, yPos, ref mv);

            match.AddRangePosition(mh.Positions);
            match.AddRangePosition(mv.Positions);

            match.Kind = MatchKind.Cross;

            return mh.Rank > 2 && mv.Rank > 2;
        }
    }
}