using System;
using CoreGameplay.Base;
using UnityEngine;

namespace CoreGameplay
{
    public class NodeBoard : MonoBehaviour
    {

        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private RectTransform boardParent;
        private NodeObject[,] _board;
        private readonly IBoardProvider _boardProvider;
        
        public NodeBoard()
        {
            _boardProvider = new RandomBoardProvider();
        }

        public void StartBoard()
        {
            ResetBoard();
        }

        public void ResetBoard()
        {
            if (_board != null)
            {
                ForeachNode(_board , (node) =>
                {
                    Destroy(node.gameObject);
                });
            }

            _board = new NodeObject[width, height];
        }
        
        public void ForeachNode(NodeObject[,] board , Action<NodeObject> actor)
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    actor(board[x,y]);
                }
            }
        }
        
        public void ForeachNode(NodeObject[,] board, Action<NodeObject[,], int, int> actor)
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    actor(board , x , y);
                }
            }
        }

        public void ForeachNode(GameObject[,] boardPrefabs, Action<GameObject, int, int> actor)
        {
            for (int x = 0; x < boardPrefabs.GetLength(0); x++)
            {
                for (int y = 0; y < boardPrefabs.GetLength(1); y++)
                {
                    actor(boardPrefabs[x,y] , x , y);
                }
            }
        }
        
        public void LoadBoard()
        {
            ResetBoard();
            var board = _boardProvider.GetNewBoard(width, height);
            
            ForeachNode(board , (nodeObject, x, y) =>
            {
                InstantiateNode(nodeObject , x , y);
            } );
        }

        private void InstantiateNode(GameObject obj , int x , int y)
        {
            if (obj == null) throw new NullReferenceException("Prefab is null");

            var o = Instantiate(obj);
            o.GetComponent<RectTransform>().SetParent(boardParent);
            var no = o.GetComponent<NodeObject>();
            no.MoveToPosition(new Vector2Int(x, y));
            no.SetBoard(this);
        }
    }
}