using System;
using CoreGameplay.Kinds;
using UnityEngine;

namespace CoreGameplay
{
    public class ExplosionManager
    {
        public static float increment = 0.025f;
        
        public void Blow(ExplosionKind kind, NodeObject main, NodeObject other, NodeBoard board)
        {
            switch (kind)
            {
                case ExplosionKind.Horizontal:
                    HorizontalExplosion(main.IndexedPosition,board);
                    break;
                case ExplosionKind.Vertical:
                    VerticalExplosion(main.IndexedPosition,board);
                    break;
                case ExplosionKind.Bomb:
                    Explode(main.IndexedPosition,board);
                    break;
                case ExplosionKind.Color:
                    DeleteColor(main.IndexedPosition, other.GetColor(),board);
                    break;
                case ExplosionKind.ColorHorizontal:
                    break;
                case ExplosionKind.ColorVertical:
                    break;
                case ExplosionKind.ColorBomb:
                    break;
                case ExplosionKind.ColorColor:
                    break;
                case ExplosionKind.BombHorizontal:
                    break;
                case ExplosionKind.BombBomb:
                    break;
                case ExplosionKind.HorizontalVertical:
                    break;
                case ExplosionKind.HorizontalHorizontal:
                    break;
                case ExplosionKind.VerticalVertical:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }

        private void DeleteColor(Vector2Int pos, NodeColor color, NodeBoard board)
        {
            board.DestroyNode(pos.x, pos.y);
            
            var nodes = board.GetBoard();
            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    if(nodes[x,y]?.GetColor() == color)
                        board.DestroyNode(x , y);
                }
            }
        }

        private void Explode(Vector2Int pos, NodeBoard board)
        {
            for (int x = 0; x < board.Width; x++)
            {
                for (int y = 0; y < board.Height; y++)
                {
                    var distance = Mathf.Sqrt(Mathf.Pow((pos.x - x), 2) + Mathf.Pow((pos.y - y), 2));
                    if (distance <= 2)
                    {
                        board.DestroyNode(x, y, distance / 10f);
                    }
                }
            }
        }

        private void VerticalExplosion(Vector2Int pos, NodeBoard board)
        {
            float counter = 0;
            for (int y = pos.y; y < board.Height; y++)
            {
                counter += increment;
                board.DestroyNode(pos.x, y, counter);
            }
            counter = 0;
            
            for (int y = pos.y; y >= 0 ; y--)
            {
                counter += increment;
                board.DestroyNode(pos.x, y, counter);
            }

        }

        private void HorizontalExplosion(Vector2Int pos, NodeBoard board)
        {
            float counter = 0;
            for (int x = pos.x; x < board.Width; x++)
            {
                counter += increment;
                board.DestroyNode(x, pos.y, counter);
            }
            counter = 0;
            
            for (int x = pos.x; x >= 0 ; x--)
            {
                counter += increment;
                board.DestroyNode(x, pos.y, counter);
            }
        }
    }
}