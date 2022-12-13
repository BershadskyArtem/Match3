using CoreGameplay.Base;
using CoreGameplay.Kinds;
using CoreGameplay.Nodes;
using UnityEngine;

namespace CoreGameplay.Matches.Rules
{
    public class InfoHorizontalMatchRule : IMatchRule<NodeInfo[,]>
    {
        public bool TryGetMatchAtPoint(NodeInfo[,] board, int xPos, int yPos, ref Match match)
        {
            var col = match.Color;
            int width = board.GetLength(0);
            bool canMatch = board[xPos, yPos].IsMatchable;
            
            if (!canMatch) return false;
            var id = board[xPos, yPos].Color;
            //left to right
            for (int x = xPos; x < width; x++)
            {
                if(board[x , yPos] == null) break;

                var canMatch2 = board[x, yPos].IsMatchable;
                if (!canMatch2) break;
                if (board[x, yPos].Color != id) break;
                
                match.AddPosition(new Vector2Int(x, yPos));
            }
            //right to left
            for (int x = xPos; x >= 0; x--)
            {
                if(board[x , yPos] == null) break;
                var canMatch2 = board[x, yPos].IsMatchable;
                if (!canMatch2) break;
                if (board[x, yPos].Color != id) break;
                
                match.AddPosition(new Vector2Int(x, yPos));
            }
            match.Kind = MatchKind.Horizontal;
            
            return match.Rank > 2;
        }
    }
}