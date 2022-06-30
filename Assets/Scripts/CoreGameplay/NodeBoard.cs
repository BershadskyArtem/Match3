using System;
using CoreGameplay.Base;
using CoreGameplay.Implementations;
using CoreGameplay.Matches.Rules;
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
        private readonly IMatchDiagnoser _matchDiagnoser;
        
        public NodeBoard()
        {
            _boardProvider = new RandomBoardProvider();
            _matchDiagnoser = new MatchDiagnoser()
                .AddMatchRule(new CrossMatchRule())
                .AddMatchRule(new HorizontalMatchRule())
                .AddMatchRule(new VerticalMatchRule());
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
            no.MoveToPosition(new Vector2Int(x, y) , true);
            no.SetBoard(this);
            _board[x, y] = no;
        }

        private void SetNodeAtPoint(GameObject obj, int x , int y)
        {
            Destroy(_board[x,y].gameObject);
            InstantiateNode(obj , x ,y);
        }

        private void VerifyBoard()
        {
            var matches = _matchDiagnoser.GetMatchesFromBoard(_board);
            foreach (var match in matches)
            {
                SetNodeAtPoint(
                    NodeFactory.Instance.GetOpposite(match.Color)
                    , match.Origin.x , 
                    match.Origin.y);
            }
        }
        
        
    }
}