using CoreGameplay.Base;
using UnityEngine;

namespace CoreGameplay.Matches.Rules
{
    public class VerticalMatchRule : IMatchRule
    {
        public bool TryGetMatchAtPoint(NodeObject[,] board, int xPos, int yPos, ref Match match)
        {
            var col = match.Color;
            int height = board.GetLength(1);
            //bottom to the top
            for (int y = yPos; y < height; y++)
            {
                if (board[xPos , y].GetColor() != col) break;
                
                match.AddPosition(new Vector2Int(xPos , y));
            }
            //from the top
            for (int y = yPos; y >= 0; y--)
            {
                if (board[xPos , y].GetColor() != col) break;
                
                match.AddPosition(new Vector2Int(xPos , y));
            }
            return match.Rank > 2;
        }
    }
}