using System;
using System.Collections;
using System.Linq;
using CoreGameplay.Base;
using CoreGameplay.BoardGravity;
using CoreGameplay.Implementations;
using CoreGameplay.Matches;
using CoreGameplay.Matches.Rules;
using Unity.Mathematics;
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
        private readonly IBoardGravityProvider _gravityProvider;
        
        public NodeBoard()
        {
            _boardProvider = new RandomBoardProvider();
            _gravityProvider = new BoardGravityProviderV2();
            _matchDiagnoser = new MatchDiagnoser()
                .AddMatchRule(new CrossMatchRule())
                .AddMatchRule(new HorizontalMatchRule())
                .AddMatchRule(new VerticalMatchRule());
            
        }

        #region Done

        

      
        public NodeObject[,] GetBoard()
        {
            return _board;
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
            VerifyBoard();
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

            if(n1 == null || n2 == null) return;
            
            if (n1.GetSwappable().CanSwap() == false || n2.GetSwappable().CanSwap() == false) return;
            if (n1.GetMatchable().CanMatch() == false || n2.GetMatchable().CanMatch() == false) return;
            
            SwipeTwoNodes(pos1 , pos2);

            var pm1 = _matchDiagnoser.GetMatchAtPoint(_board, pos1.x, pos1.y);
            var pm2 = _matchDiagnoser.GetMatchAtPoint(_board, pos2.x, pos2.y);

            if (!Match.isZero(pm1))
            {
                Debug.Log("SF11");
                DestroyMatch(pm1);
            }
            if (!Match.isZero(pm2))
            {
                Debug.Log("SF11");
                
                DestroyMatch(pm2);
            }

            if (!Match.isZero(pm1) || !Match.isZero(pm2))
            {
                ApplyGravity();
            }
            SwipeTwoNodes(pos1 , pos2);
        }
        
        #endregion
        
        
        private void ApplyGravity()
        {
           var counter = _gravityProvider.ApplyGravity(this);
           Debug.Log($"Gravity applied to {counter} objects");
           int stop = counter > 0 ? 1 : 0;
           while (counter > 0 && stop < 10)
           {
               Debug.Log($"Gravity applied to {counter} objects");
               counter = _gravityProvider.ApplyGravity(this);
               stop++;
           }
           if (stop > 0)
           {
               CheckMatches();
           }
           
        }

        private void CheckMatches()
        {
            var matches = _matchDiagnoser.GetMatchesFromBoard(_board);
            Debug.Log($"{matches.Count()} matches found.");
            foreach (var match in matches)
            {
                DestroyMatch(match);
            }
            ApplyGravity();
        }

        private IEnumerator ApplyGravityContinious()
        {
            while (true)
            {
                _gravityProvider.ApplyGravity(this);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator CheckMatchesContinious()
        {
            while (true)
            {
                var matches = _matchDiagnoser.GetMatchesFromBoard(_board);

                foreach (var match in matches)
                {
                    DestroyMatch(match);
                }
                
                yield return new WaitForSeconds(0.2f);
            }
        }

        public bool IsInsideBoard(Vector2Int pos) => IsInsideBoard(pos.x , pos.y);
        public bool IsInsideBoard(int x , int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
        public void SwipeTwoNodes(Vector2Int pos1 , Vector2Int pos2)
        {
            //swipe visuals
            _board[pos1.x, pos1.y]?.MoveToPosition(pos2 , false);
            _board[pos2.x, pos2.y]?.MoveToPosition(pos1 , false);
            
            //swipe in array
            (_board[pos1.x, pos1.y], _board[pos2.x, pos2.y]) = (_board[pos2.x, pos2.y], _board[pos1.x, pos1.y]);
        }
        private void DestroyMatch(Match match)
        {
            var poss = match.Positions;
            foreach (var pos in poss)
            {
                DestroyNode(pos.x , pos.y);
            }
        }
        private void DestroyNode(int x , int y)
        {
            if(!IsInsideBoard(x,y)) return;
            _board[x,y]?.DestroyNode();
            _board[x, y] = null;
        }

        private void CheckInternalBoard()
        {
            ForeachNode(_board, (nodes, x, y) =>
            {
                var n = nodes[x, y];
                if (n.GetPos() != new Vector2Int(x, y))
                {
                    n.MoveToPosition(new Vector2Int(x,y) , false);
                }
            });
        }
        
    }
}