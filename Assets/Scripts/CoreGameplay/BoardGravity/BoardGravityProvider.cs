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
            if (board == null) return 0;
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

                    var rightOff = pos + Vector2Int.right;
                    var leftOff = pos + Vector2Int.left;
                    
                    //var dd = pos + _down;
                    Vector2Int dd = pos;
                    int c = 0;
                    for (int i = y - 1; i > 0; i--)
                    {
                        if ( !(nodeBoard.IsInsideBoard(dr) && board[x, i] == null)) break;
                        dd += _down;
                    }

                    if (dd != pos)
                    {
                        nodeBoard.SwipeTwoNodes(pos , dd);
                        counter++;
                        continue;
                    }
                    
                   
                    if (nodeBoard.IsInsideBoard(rightOff))
                    {
                        if (board[rightOff.x, rightOff.y] == null)
                        {
                            if (nodeBoard.IsInsideBoard(dr) && board[dr.x, dr.y] == null)
                            {
                                nodeBoard.SwipeTwoNodes(pos , dr);
                                _road = !_road;
                                counter++;
                                continue;
                            }
                        }
                    }

                    if (nodeBoard.IsInsideBoard(leftOff))
                    {
                        if (board[leftOff.x, leftOff.y] == null)
                        {
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
            }

            return counter;
        }
    }
}