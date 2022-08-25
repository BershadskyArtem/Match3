using UnityEngine;

namespace CoreGameplay
{
    public class LineSpawner : INodeSpawner
    {

        public int Spawn(NodeBoard nodeBoard)
        {
            var board = nodeBoard.GetBoard();
            int heightIndex = board.GetLength(1) - 1;
            int xDim = board.GetLength(0);
            int counter = 0;
            for (int x = 0; x < xDim ; x++)
            {
                if (board[x, heightIndex] == null)
                {
                    counter++;
                    nodeBoard.Spawn(x,heightIndex , x , heightIndex+1,NodeFactory.Instance.GetRandomPrefab());
                }    
            }

            return counter;
        }
        
    }
}