﻿using CoreGameplay.Base;
using CoreGameplay.Kinds;
using UnityEngine;

namespace CoreGameplay.Matches.Rules
{
    public class HorizontalMatchRule:IMatchRule<NodeObject[,]>
    {
        public bool TryGetMatchAtPoint(NodeObject[,] board,  int xPos, int yPos, ref Match match)
        {
            var col = match.Color;
            int width = board.GetLength(0);
            var swap = board[xPos, yPos].GetMatchable();
            if (!swap.CanMatch()) return false;
            var id = swap.GetID();
            //left to right
            for (int x = xPos; x < width; x++)
            {
                if(board[x , yPos] == null) break;

                var swap2 = board[x, yPos].GetMatchable();
                if (!swap2.CanMatch()) break;
                if (swap2.GetID() != id) break;
                
                match.AddPosition(new Vector2Int(x, yPos));
            }
            //right to left
            for (int x = xPos; x >= 0; x--)
            {
                if(board[x , yPos] == null) break;
                var swap2 = board[x, yPos].GetMatchable();
                if (!swap2.CanMatch()) break;
                if (swap2.GetID() != id) break;
                
                match.AddPosition(new Vector2Int(x, yPos));
            }
            match.Kind = MatchKind.Horizontal;
            
            return match.Rank > 2;
        }
    }
}