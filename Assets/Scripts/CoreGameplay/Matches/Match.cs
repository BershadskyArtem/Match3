using System.Collections.Generic;
using CoreGameplay.Kinds;
using UnityEngine;

namespace CoreGameplay.Matches
{
    public class Match
    {
        public readonly Vector2Int Origin;
        public readonly List<Vector2Int> Positions;
        public readonly NodeColor Color;
        public int Rank => Positions.Count;

        public static Match Zero => new Match();
        public MatchKind Kind { get; set; }

        public static bool isZero(Match match)
        {
            return match.Rank == 0;
        }
        
        private Match()
        {
            Origin = new Vector2Int();
            Positions = new List<Vector2Int>();
            Color = NodeColor.Unknown;
        }

        public Match(Vector2Int origin , NodeColor color)
        {
            Origin = origin;
            Positions = new List<Vector2Int>();
            Positions.Add(Origin);
            Color = color;
        }

        public void AddPosition(Vector2Int pos)
        {
            if(Positions.Contains(pos)) return;
            Positions.Add(pos);
        }
        
        public void AddRangePosition(List<Vector2Int> poss)
        {
            foreach (Vector2Int pos in poss)
            {
                AddPosition(pos);
            }
        }

        public Match CloneWithoutPositions()
        {
            return new Match(this.Origin , this.Color);
        }
        
    }
}