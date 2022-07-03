using UnityEngine;

namespace CoreGameplay.BoardGravity
{
    public class BoardGravityProvider : IBoardGravityProvider
    {

        private Vector2Int _down = Vector2Int.down;
        private Vector2Int _downLeft = Vector2Int.down + Vector2Int.left;
        private Vector2Int _downRight = Vector2Int.down + Vector2Int.right;
        private bool _road = true;
        
        public int ApplyGravity(NodeBoard nodeBoard)
        {
            var board = nodeBoard.GetBoard();
            int width = board.GetLength(0);
            int height = board.GetLength(1);
            int counter = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var node = board[x, y];
                    if(node == null) continue;
                    Vector2Int pos = new Vector2Int(x, y);
                    var dl = pos + _downLeft;
                    var dr = pos + _downRight;
                    var dd = pos + _down;
                    if ( nodeBoard.IsInsideBoard(dd) && board[dd.x, dd.y] == null)
                    {
                        nodeBoard.SwipeTwoNodes(pos , dd);
                        counter++;
                        continue;
                    }
                    if (_road)
                    {
                        if (nodeBoard.IsInsideBoard(dr) && board[dr.x, dr.y] == null)
                        {
                            nodeBoard.SwipeTwoNodes(pos , dr);
                            _road = !_road;
                            counter++;
                            continue;
                        }
                        if (nodeBoard.IsInsideBoard(dl) && board[dl.x, dl.y] == null)
                        {
                            nodeBoard.SwipeTwoNodes(pos , dl);
                            _road = !_road;
                            counter++;
                            continue;
                        }
                    }
                }
            }

            return counter;
        }
    }
}