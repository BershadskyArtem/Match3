using System.Collections.Generic;
using System.Linq;
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

        public Vector2Int Center
        {
            get
            {
                if (Kind != MatchKind.Cross)
                {
                    if (Kind == MatchKind.Horizontal)
                    {
                        Positions.OrderBy(v => v.x);
                        if (Rank == 3) 
                            return Positions[1];
                        if (Rank == 5)
                            return Positions[2];
                        
                        var lowest = Positions.First();
                        return new Vector2Int( lowest.x + Mathf.CeilToInt(Positions.Count / 2f) - 1  , Origin.y);
                    }else if (Kind == MatchKind.Vertical)
                    {
                        Positions.OrderBy(v => v.y);
                        if (Rank == 3) 
                            return Positions[1];
                        if (Rank == 5)
                            return Positions[2];
                        
                        var leftest = Positions.First();
                        return new Vector2Int( Origin.x , leftest.y + Mathf.CeilToInt(Positions.Count / 2f) - 1 );
                    }
                }
                return Origin;
            }
        }

        public GameObject BombPrefab
        {
            get
            {
                if (Kind == MatchKind.Cross)
                {
                    return NodeFactory.Instance.GetBomb(BombKind.Bomb);
                }
                else if (Kind == MatchKind.Horizontal)
                {
                    if (Rank == 4)
                    {
                        return NodeFactory.Instance.GetBomb(BombKind.Vertical);
                    }
                    else if(Rank > 4)
                    {
                        return NodeFactory.Instance.GetBomb(BombKind.Color);
                    }
                }else if (Kind == MatchKind.Vertical)
                {
                    if (Rank == 4)
                    {
                        return NodeFactory.Instance.GetBomb(BombKind.Horizontal);
                    }
                    else if(Rank > 4)
                    {
                        return NodeFactory.Instance.GetBomb(BombKind.Color);
                    }
                }

                return null;
            }
        }

    }
}