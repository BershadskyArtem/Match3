using System;
using System.Linq;
using CoreGameplay.Base;
using CoreGameplay.Implementations;
using CoreGameplay.Matches;
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
            VerifyBoard();
            VerifyBoard();
            VerifyBoard();
            ForeachNode(_board , (board, x, y) =>
            {
                MoveNodeToCoord(board[x, y], x, y);
            });
            
            
        }

        private void InstantiateNode(GameObject obj , int x , int y)
        {
            if (obj == null) throw new NullReferenceException("Prefab is null");

            var o = Instantiate(obj);
            o.GetComponent<RectTransform>().SetParent(boardParent , false);
            var no = o.GetComponent<NodeObject>();
            no.SetBoard(this);
            _board[x, y] = no;
        }

        private void MoveNodeToCoord(NodeObject node , int x , int y)
        {
            node.MoveToPosition(new Vector2Int(x, y) , true);
        }

        private void SetNodeAtPoint(GameObject obj, int x , int y)
        {
            Destroy(_board[x,y].gameObject);
            InstantiateNode(obj , x ,y);
        }

        private void VerifyBoard()
        {
            
            var matches = _matchDiagnoser.GetMatchesFromBoard(_board);
            Debug.Log($"Verified = {matches.Count()}");
            foreach (var match in matches)
            {
                SetNodeAtPoint(
                    NodeFactory.Instance.GetOpposite(match.Color)
                    , match.Origin.x , 
                    match.Origin.y);
            }
        }

        public void TrySwipeTwoNodes(Vector2Int pos1 , Vector2Int pos2)
        {
            if(!IsInsideBoard(pos1) || !IsInsideBoard(pos2)) return;
            
            var n1 = _board[pos1.x , pos1.y];
            var n2 = _board[pos2.x , pos2.y];
            
            if(!n1.GetSwappable().CanSwap() || !n2.GetSwappable().CanSwap()) return;
            if(!n1.GetMatchable().CanMatch() || !n2.GetMatchable().CanMatch()) return;
            
            SwipeTwoNodes(pos1 , pos2);

            var pm1 = _matchDiagnoser.GetMatchAtPoint(_board, pos1.x, pos1.y);
            var pm2 = _matchDiagnoser.GetMatchAtPoint(_board, pos2.x, pos2.y);
            
            if(!Match.isZero(pm1) || !Match.isZero(pm2)) return;
            
            SwipeTwoNodes(pos1 , pos2);
        }

        private bool IsInsideBoard(Vector2Int pos) => IsInsideBoard(pos.x , pos.y);
        private bool IsInsideBoard(int x , int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
        private void SwipeTwoNodes(Vector2Int pos1 , Vector2Int pos2)
        {
            //swipe visuals
            _board[pos1.x, pos1.y].MoveToPosition(pos2 , false);
            _board[pos2.x, pos2.y].MoveToPosition(pos1 , false);
            
            //swipe in array
            (_board[pos1.x, pos1.y], _board[pos2.x, pos2.y]) = (_board[pos2.x, pos2.y], _board[pos1.x, pos1.y]);
            
        }
        
    }
}