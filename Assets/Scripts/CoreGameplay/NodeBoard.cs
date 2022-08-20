using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using CoreGameplay.Base;
using CoreGameplay.BoardGravity;
using CoreGameplay.Implementations;
using CoreGameplay.Kinds;
using CoreGameplay.Matches;
using CoreGameplay.Matches.Rules;
using UnityEngine;

namespace CoreGameplay
{
    public class NodeBoard : MonoBehaviour
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private Transform boardTransformParent;
        
        private NodeObject[,] _board;
        private readonly IBoardProvider _boardProvider;
        private readonly IMatchDiagnoser _matchDiagnoser;
        private readonly IBoardGravityProvider _gravityProvider;
        private readonly INodeSpawner _nodeSpawner;

        private NodeObject _activeNode;
        

        public NodeBoard()
        {
            _nodeSpawner = new LineSpawner();
            _boardProvider = new RandomBoardProvider();
            _gravityProvider = new BoardGravityProviderV2();
            _matchDiagnoser = new MatchDiagnoser()
                .AddMatchRule(new CrossMatchRule())
                .AddMatchRule(new HorizontalMatchRule())
                .AddMatchRule(new VerticalMatchRule());
        }

        private void OnEnable()
        {
            SwipesRegistrar.OnSwipe += HandleSwipes;
            SwipesRegistrar.OnNewNodeSelected += HandleNewNode;
            // Lean.Touch.LeanTouch.OnFingerSwipe += HandleSwipesT;
        }

        private void HandleNewNode(NodeObject obj)
        {
            this._activeNode = obj;
        }

        private void OnDisable()
        {
            SwipesRegistrar.OnSwipe -= HandleSwipes;
           // Lean.Touch.LeanTouch.OnFingerSwipe -= HandleSwipesT;
        }

      
        private void HandleSwipes(Direction direction)
        {
            if (_activeNode == null)
            {
                //Debug.LogError("Active node is null");
                return;
            }
            var pos = _activeNode.IndexedPosition;
            switch (direction)
            {
                case Direction.Down:
                    TrySwipeTwoNodes(pos , pos + Vector2Int.down);
                    break;
                case Direction.Left:
                    TrySwipeTwoNodes(pos , pos + Vector2Int.left);
                    break;
                case Direction.Right:
                    TrySwipeTwoNodes(pos , pos + Vector2Int.right);
                    break;
                case Direction.Up:
                    TrySwipeTwoNodes(pos , pos + Vector2Int.up);
                    break;
            }
        }

        #region Done

   
      
        public NodeObject[,] GetBoard()
        {
            return _board;
        }
        
        private void ResetBoard()
        {
            if (_board != null)
            {
                ForeachNode(_board , (node) =>
                {
                    Destroy(node?.gameObject);
                });
            }

            _board = new NodeObject[width, height];
        }
        
        private void ForeachNode(NodeObject[,] board , Action<NodeObject> actor)
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    actor(board[x,y]);
                }
            }
        }
        
        private void ForeachNode(NodeObject[,] board, Action<NodeObject[,], int, int> actor)
        {
            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    actor(board , x , y);
                }
            }
        }

        private void ForeachNode(GameObject[,] boardPrefabs, Action<GameObject, int, int> actor)
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
            ForeachNode(board, InstantiateNode);
            VerifyBoard();
            VerifyBoard();
            VerifyBoard();
            VerifyBoard();
            VerifyBoard();
            ForeachNode(_board , (board, x, y) =>
            {
                MoveNodeToCoord(board[x, y], x, y);
            });

            _activeNode = _board[0, 0];

        }

        private void InstantiateNode(GameObject obj , int x , int y)
        {
            if (obj == null) throw new NullReferenceException("Prefab is null");

            var o = Instantiate(obj);
            //o.GetComponent<RectTransform>().SetParent(boardParent , false);
            o.GetComponent<Transform>().SetParent(boardTransformParent , false);
            var no = o.GetComponent<NodeObject>();
            no.SetBoard(this);
            _board[x, y] = no;
        }

        private void MoveNodeToCoord(NodeObject node , int x , int y)
        {
            node.FallToPos(new Vector2Int(x, y));
        }

        private void SetNodeAtPoint(GameObject obj, int x , int y)
        {
            Destroy(_board[x,y].gameObject);
            InstantiateNode(obj , x ,y);
        }

        private void VerifyBoard()
        {
            var matches = _matchDiagnoser.GetMatchesFromBoard(_board);
            //Debug.Log($"Verified = {matches.Count()}");
            foreach (var match in matches)
            {
                SetNodeAtPoint(
                    NodeFactory.Instance.GetOpposite(match.Color)
                    , match.Origin.x , 
                    match.Origin.y);
            }
        }

        #endregion
        public void TrySwipeTwoNodes(Vector2Int pos1 , Vector2Int pos2)
        {
            if(!IsInsideBoard(pos1) || !IsInsideBoard(pos2)) return;
            
            var n1 = _board[pos1.x , pos1.y];
            var n2 = _board[pos2.x , pos2.y];

            if(n1 == null || n2 == null) return;

            if (!(n1.GetSwappable().CanSwap() && n2.GetSwappable().CanSwap())) return;

            SwipeTwoNodes(pos1 , pos2);

            StartCoroutine(CheckSwipeCoroutine(pos1, pos2, n1, n2));
            
            GC.Collect();
        }


        /// <summary>
        /// DONT CALL THIS UNLESS YOU KNOW WHAT IT IS
        /// </summary>
        /// <returns></returns>
        private IEnumerator CheckSwipeCoroutine(Vector2Int pos1 , Vector2Int pos2 , NodeObject n1, NodeObject n2)
        {
            yield return new WaitForSeconds(AnimationNumbers.SwapSpeed);
            
            if (n1.IsBomb)
            {
                ExplodeRegion(n1.BombRank - 3 , n1.IndexedPosition);
                yield break;
            }
            else if (n2.IsBomb)
            {
                ExplodeRegion(n2.BombRank - 3 , n2.IndexedPosition);
                yield break;
            }

            if (!(n1.GetMatchable().CanMatch() && n2.GetMatchable().CanMatch())) yield break;

            var pm1 = _matchDiagnoser.GetMatchAtPoint(_board, pos1.x, pos1.y);
            var pm2 = _matchDiagnoser.GetMatchAtPoint(_board, pos2.x, pos2.y);

            if (!Match.isZero(pm1))
            {
                DestroyMatch(pm1);
            }
            if (!Match.isZero(pm2))
            {
                DestroyMatch(pm2);
            }

            if (!Match.isZero(pm1) || !Match.isZero(pm2))
            {
                Invoke(nameof(ApplyGravity),AnimationNumbers.GravityApplyDelay); 
                yield break;
            }
            SwipeTwoNodes(pos1 , pos2);
            
        }
        
        private void ExplodeRegion(int radius , Vector2Int origin)
        {
            DestroyNode(origin.x , origin.y);

            switch (radius)
            {
                case 1:
                {
                    var y = origin.y;
                    for (int x = 0; x < _board.GetLength(0); x++)
                    {
                        if(_board[x, y] == null) continue;
                        if (_board[x, y].IsBomb)
                        {
                            ExplodeRegion(_board[x, y].BombRank - 3, _board[x, y].IndexedPosition);
                        }else
                            DestroyNode(x , y);
                    }
                    break;
                }
                default:
                {
                    ForeachNode(_board, (node , x , y) =>
                    {
                        if (node[x, y] != null)
                        {
                            var l = Mathf.Sqrt(Mathf.Pow((x - origin.x), 2) + Mathf.Pow((y - origin.y), 2));
                            if (l <= radius)
                            {
                                if (node[x, y].IsBomb)
                                {
                                    ExplodeRegion(node[x, y].BombRank - 3, node[x, y].IndexedPosition);
                                }else
                                    DestroyNode(x , y);
                            }    
                        }
                
                    });
                    break;
                }
            }
            
            ApplyGravity();
        }

        private void ApplyGravity()
        {
            _nodeSpawner.Spawn(this);
            var counter = _gravityProvider.ApplyGravity(this);
            var appliedCounter = 0;
            while (counter > 0)
            {
                _nodeSpawner.Spawn(this);
                counter = _gravityProvider.ApplyGravity(this);
                //Debug.Log($"Gravity applied to {counter} objects");
                appliedCounter++;
            }
            CheckBoard();
            if (appliedCounter > 0)
            {
                Invoke(nameof(CheckBoard),AnimationNumbers.SwipeCheckDelay); 
            }
            
        }
        private void CheckBoard()
        {
            var matches = _matchDiagnoser.GetMatchesFromBoard(_board);
            var iMatches = matches as Match[] ?? matches.ToArray();
            foreach (var match in iMatches)
            {
                DestroyMatch(match);
            }
            if (iMatches.Any())
            {
                Invoke(nameof(ApplyGravity) , AnimationNumbers.GravityApplyDelay);
            }
        }
        public bool IsInsideBoard(Vector2Int pos) => IsInsideBoard(pos.x , pos.y);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsInsideBoard(int x , int y)
        {
            return x >= 0 && x < width && y >= 0 && y < height;
        }
        public void SwipeTwoNodes(Vector2Int pos1 , Vector2Int pos2 , bool useGravityTime = false)
        {
        
            var isInside = this.IsInsideBoard(pos1) && this.IsInsideBoard(pos2);
            if(!isInside) return;
            //swipe visuals
            //_board[pos1.x, pos1.y]?.MoveToPosition(pos2 , useGravityTime);
           // _board[pos2.x, pos2.y]?.MoveToPosition(pos1 , useGravityTime);
           
           //_board[pos1.x, pos1.y]?.MoveToPositionLerp(pos2 , useGravityTime);
           //_board[pos2.x, pos2.y]?.MoveToPositionLerp(pos1 , useGravityTime);
          
           _board[pos1.x, pos1.y]?.SwapToPos(pos2);
           _board[pos2.x, pos2.y]?.SwapToPos(pos1);
            
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

            if (match.Rank >= 4)
            {
                SetNode(match.Origin , NodeFactory.Instance.GetBomb(match.Rank) ,false);
            }
                
       
        }
        private void DestroyNode(int x , int y)
        {
            if(!IsInsideBoard(x,y)) return;
            _board[x,y]?.DestroyNode();
            _board[x, y] = null;
        }
        public void SetNode(Vector2Int pos , GameObject node)
        {
            DestroyNode(pos.x , pos.y);
            if (node == null) return;
            InstantiateNode(node ,  pos.x , pos.y);
        }
        public void SetNode(Vector2Int pos , GameObject node , bool isSpawned)
        {
            DestroyNode(pos.x , pos.y);
            InstantiateNode(node ,  pos.x , pos.y);
           // _board[pos.x , pos.y].MoveToPosition(pos , false);
            _board[pos.x , pos.y].SwapToPos(pos);
        }
        
    }
}