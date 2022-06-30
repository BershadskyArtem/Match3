using CoreGameplay.Base;
using UnityEngine;

namespace CoreGameplay.Matches.Rules
{
    public class HorizontalMatchRule:IMatchRule
    {
        public bool TryGetMatchAtPoint(NodeObject[,] board,  int xPos, int yPos, ref Match match)
        {
            var col = match.Color;
            int width = board.GetLength(0);
            //left to right
            for (int x = xPos; x < width; x++)
            {
                if (board[x, yPos].GetColor() != col) break;
                
                match.AddPosition(new Vector2Int( x , yPos));
            }
            //right to left
            for (int x = xPos; x >= 0; x--)
            {
                if (board[x, yPos].GetColor() != col) break;
                
                match.AddPosition(new Vector2Int( x , yPos));
            }
            return match.Rank > 2;
        }
    }
}