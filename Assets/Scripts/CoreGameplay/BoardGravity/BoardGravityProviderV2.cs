using UnityEngine;

namespace CoreGameplay.BoardGravity
{
    public class BoardGravityProviderV2 : IBoardGravityProvider
    {
        private Vector2Int _up =      Vector2Int.up;
        private Vector2Int _upLeft =  Vector2Int.up + Vector2Int.left;
        private Vector2Int _upRight = Vector2Int.up + Vector2Int.right;
        
        public int ApplyGravity(NodeBoard nodeBoard)
        {
            var board = nodeBoard.GetBoard();
            if (board == null) return 0;
            int width = board.GetLength(0);
            int height = board.GetLength(1);
            int counter = 0;
            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (board[x, y] != null) continue;
                    Vector2Int pos = new Vector2Int(x,y);
                    bool up = false;
                    for (int i = y + 1; i < height; i++)
                    {
                        if (board[x, i] == null) continue;
                        nodeBoard.SwipeTwoNodes(pos , pos + Vector2Int.up , true);
                        counter++;
                        up = true;
                        break;
                    }
                    //if (up) continue;
                    
                }
            }

            return counter;
        }
        
    }
}