using UnityEngine;

namespace CoreGameplay
{
    public class LineSpawner : INodeSpawner
    {

        public void Spawn(NodeBoard nodeBoard)
        {
            var board = nodeBoard.GetBoard();
            int heightIndex = board.GetLength(1) - 1;
            int xDim = board.GetLength(0);
            
            for (int x = 0; x < xDim ; x++)
            {
                if (board[x, heightIndex] == null)
                {
                    nodeBoard.SetNode(new Vector2Int(x , heightIndex) , NodeFactory.Instance.GetRandomPrefab() , true);
                   nodeBoard.SpawnNode(new Vector2Int(x, heightIndex + 1), new Vector2Int(x, heightIndex),
                        NodeFactory.Instance.GetRandomPrefab());
                   // nodeBoard.Spawn(new Vector2Int(x, heightIndex + 1), new Vector2Int(x, heightIndex),
                   //     NodeFactory.Instance.GetRandomPrefab());
                }    
            }

        }
        
    }
}