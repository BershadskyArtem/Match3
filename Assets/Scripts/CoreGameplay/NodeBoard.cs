using System.Linq;
using CoreGameplay.Base;
using CoreGameplay.BoardGravity;
using CoreGameplay.Implementations;
using CoreGameplay.Kinds;
using CoreGameplay.Matches;
using CoreGameplay.Matches.Rules;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Direction = CoreGameplay.Kinds.Direction;

namespace CoreGameplay
{
    public class NodeBoard : MonoBehaviour
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private Transform boardTransformParent;
        
        private NodeObject[,] _board;
        private readonly IBoardProvider _boardProvider;
        private readonly IMatchDiagnoser<NodeObject[,]> _matchDiagnoser;
        private readonly IBoardGravityProvider _gravityProvider;
        private readonly INodeSpawner _nodeSpawner;
        private readonly ExplosionManager _explosions;
      

        private NodeObject _activeNode;

        private Vector2Int _lastMain;
        private Vector2Int _lastOther;
        
        public NodeBoard()
        {
            _explosions = new ExplosionManager();
            _nodeSpawner = new LineSpawner();
            _boardProvider = new RandomBoardProvider(new InfoMatchDiagnoser());
            _gravityProvider = new BoardGravityProviderV2();
            _matchDiagnoser = new MatchDiagnoser()
                .AddMatchRule(new CrossMatchRule())
                .AddMatchRule(new HorizontalMatchRule())
                .AddMatchRule(new VerticalMatchRule());
        }

        public int Height => height;
        public int Width => width;

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
                    TrySwapNodes(pos , pos + Vector2Int.down);
                    break;
                case Direction.Left:
                    TrySwapNodes(pos , pos + Vector2Int.left);
                    break;
                case Direction.Right:
                    TrySwapNodes(pos , pos + Vector2Int.right);
                    break;
                case Direction.Up:
                    TrySwapNodes(pos , pos + Vector2Int.up);
                    break;
            }
        }
        
        public NodeObject[,] GetBoard()
        {
            return _board;
        }

        public void RandomBoard()
        {
            if(_board != null)
                DestroyBoard();
            
            _board ??= new NodeObject[width, height];
            
            var prefabs = _boardProvider.GetNewBoard(width, height);
            

            for (int x = 0; x < _board.GetLength(0); x++)
            {
                for (int y = 0; y < _board.GetLength(1); y++)
                {
                        _board[x,y]?.DestroyNode();
                        _board[x,y] = null;
                        Spawn(x, y, x, 15, prefabs[x,y]);
                }
            }

            //VerifyBoard(_board);
        }

        private void DestroyBoard()
        {
            for (int x = 0; x < _board.GetLength(0); x++)
            {
                for (int y = 0; y < _board.GetLength(1); y++)
                {
                    _board[x,y]?.DestroyNode();
                    _board[x, y] = null;
                }
            }
        }


        public void VerifyBoard(NodeObject[,] board)
        {
            int counter = 1;
            int stupid = 0;

            while (counter !=0 && stupid < 50)
            {
                var matches = _matchDiagnoser.GetMatchesFromBoard(board);
                if (matches.Count() == 0) return;
                stupid++;
                foreach (var match in matches)
                {
                    var prefab = 
                        NodeFactory.Instance.GetOpposite(board[match.Center.x, match.Center.y].GetColor());
                    board[match.Center.x,match.Center.y].DestroyWithNoAnimation();
                    
                    Spawn(match.Center.x, match.Center.y, match.Center.x, 15, prefab);
                    
                    //var inst = Instantiate(prefab);
                    //var nodeObject = inst.GetComponent<NodeObject>();
                    //nodeObject.SetBoard(this);
                    //nodeObject.ForcePosition(match.Center);
                    //board[match.Center.x, match.Center.y] = nodeObject;
                }
            }

            if (stupid >= 50)
            {
                Debug.LogError("Stupid Lock");
            }
        }
        

        public void Spawn(int xIndex , int yIndex, int xPos , int yPos , GameObject prefab)
        {
            var inst = Instantiate(prefab);
            var nodeObject = inst.GetComponent<NodeObject>();
            nodeObject.SetBoard(this);
            nodeObject.SpawnFrom(new Vector2Int(xPos, yPos), new Vector2Int(xIndex, yIndex));
            _board[xIndex, yIndex] = nodeObject;
        }

        public void TrySwapNodes(Vector2Int mainPos , Vector2Int otherPos)
        { 
            var main = _board[mainPos.x, mainPos.y]; 
            var other = _board[otherPos.x, otherPos.y];

            var cancelMainPre = main.PreSwipeReaction(this, other);
            var cancelOtherPre = other.PreSwipeReaction(this, other);
            
            if(cancelMainPre || cancelOtherPre) return;

            if (!(main.GetSwappable().CanSwap() && other.GetSwappable().CanSwap())) return;
            main.SwapToPos(otherPos);
            other.SwapToPos(mainPos);
            
            //swap
            (_board[mainPos.x, mainPos.y], _board[otherPos.x, otherPos.y]) = (_board[otherPos.x, otherPos.y], _board[mainPos.x, mainPos.y]);

            var cancelMain = main.SwipeReaction(this, other);
            
            if(cancelMain) return;
            
            var cancelOther = other.SwipeReaction(this, other);
            
            if(cancelOther) return;

            _lastMain = mainPos;
            _lastOther = otherPos;
            
            Invoke(nameof(CheckAndApply) , AnimationNumbers.SwipeCheckDelay);
        }

        private void CheckAndApply()
        {
            var otherMatch = _matchDiagnoser.GetMatchAtPoint(_board, _lastOther.x, _lastOther.y);
            var mainMatch = _matchDiagnoser.GetMatchAtPoint(_board, _lastMain.x, _lastMain.y);

            bool isAtLeastOneMatch = false;
            
            if (!Match.isZero(otherMatch))
            {
                ResolveMatch(otherMatch, true);
                isAtLeastOneMatch = true;
            }

            if (!Match.isZero(mainMatch))
            {
                ResolveMatch(mainMatch, true);
                isAtLeastOneMatch = true;
            }
            
            if (isAtLeastOneMatch)
            {
                Invoke(nameof(ApplyGravity) , AnimationNumbers.GravityApplyDelay);
                return;
            }
            
            Invoke(nameof(CancelSwipe) , AnimationNumbers.SwipeCheckDelay / 4f);
        }

        private void ApplyGravity()
        {
            int applied = _gravityProvider.ApplyGravity(this);
            _nodeSpawner.Spawn(this);
            applied = _gravityProvider.ApplyGravity(this);
            while (applied != 0)
            {
                _nodeSpawner.Spawn(this);
                applied = _gravityProvider.ApplyGravity(this);
            }

            Invoke(nameof(CheckAndDestroyMatches) , AnimationNumbers.SwipeCheckDelay);
        }

        public void CheckAndDestroyMatches()
        {
            var matches = _matchDiagnoser.GetMatchesFromBoard(_board);
            if (!matches.Any()) return;
            foreach (var match in matches)
            {
                ResolveMatch(match);
            }
            Invoke(nameof(ApplyGravity) , AnimationNumbers.GravityApplyDelay);
        }

        private void CancelSwipe()
        {
            _board[_lastMain.x, _lastMain.y].SwapToPos(_lastOther);
            _board[_lastOther.x, _lastOther.y].SwapToPos(_lastMain);
            //swap
            (_board[_lastMain.x, _lastMain.y], _board[_lastOther.x, _lastOther.y]) = (_board[_lastOther.x, _lastOther.y], _board[_lastMain.x, _lastMain.y]);
        }

        private void ResolveMatch(Match mainMatch, bool useOrigins = false)
        {
            foreach (var pos in mainMatch.Positions)
            {
                _board[pos.x, pos.y]?.ResolveAsPartOfMatch(useOrigins ? mainMatch.Origin : mainMatch.Center);
                _board[pos.x, pos.y] = null;
            }

            var prefab = mainMatch.BombPrefab;
            if(prefab == null) return;

            if (useOrigins)
            {
                _board[mainMatch.Origin.x,mainMatch.Origin.y]?.DestroyWithNoAnimation();
                var inst = Instantiate(prefab);
                var nodeObject = inst.GetComponent<NodeObject>();
                nodeObject.SetBoard(this);
                nodeObject.ForcePosition(mainMatch.Origin);
                _board[mainMatch.Origin.x, mainMatch.Origin.y] = nodeObject;
            }
            else
            {
                _board[mainMatch.Center.x,mainMatch.Center.y]?.DestroyWithNoAnimation();
                var inst = Instantiate(prefab);
                var nodeObject = inst.GetComponent<NodeObject>();
                nodeObject.SetBoard(this);
                nodeObject.ForcePosition(mainMatch.Center);
                _board[mainMatch.Center.x, mainMatch.Center.y] = nodeObject;
            }
            
            
        }

        public void Boom(BombKind bombKind , NodeObject main, NodeObject other = null)
        {
            switch (bombKind)
            {
                case BombKind.Horizontal:
                {
                    Boom(ExplosionKind.Horizontal,main, other);
                    break;
                }
                
                case BombKind.Vertical:
                {
                    Boom(ExplosionKind.Vertical,main, other);
                    break;
                }
                
                case BombKind.Bomb:
                {
                    Boom(ExplosionKind.Bomb,main, other);
                    break;
                }
                
                case BombKind.Color:
                {
                    Boom(ExplosionKind.Color ,main, other);
                    break;
                }
            }
        }

        private ExplosionKind _lastExplosionKind;
        private NodeObject _lastMainNode;
        private NodeObject _lastOtherNode;

        public void Boom(ExplosionKind boomKind ,NodeObject main, NodeObject other = null)
        {
            _lastMainNode = main;
            _lastOtherNode = other;
            _lastExplosionKind = boomKind;
            Invoke(nameof(BoomInternal) , AnimationNumbers.SwipeCheckDelay);
        }

        private void BoomInternal()
        {
            _explosions.Blow(_lastExplosionKind, _lastMainNode, _lastOtherNode, this);
            Invoke(nameof(ApplyGravity) , AnimationNumbers.GravityApplyDelay + AnimationNumbers.SwipeCheckDelay / 2f);
        }

        public void DestroyNode(Vector2Int pos) => DestroyNode(pos.x, pos.y);

        public void DestroyNode(int x , int y)
        {
            _board[x, y]?.ResolveAsPartOfMatch();
            _board[x,y] = null;
        }
        
        public void DestroyNode(int x , int y, float delay)
        {
            _board[x, y]?.ResolveAsPartOfMatch(delay);
            _board[x,y] = null;
        }

        public bool IsInsideBoard(Vector2Int pos) => IsInsideBoard(pos.x, pos.y);

        public bool IsInsideBoard(int x, int y)
        {
            return (x >= 0 && x < width) && (y >= 0 && y < height);
        }

        public void FallNode(Vector2Int from, Vector2Int dest) => FallNode(from.x, from.y, dest.x, dest.y);

        public void FallNode(int xFrom , int yFrom , int xTo , int yTo)
        {
            _board[xFrom, yFrom]?.FallToPos(new Vector2Int(xTo,yTo));
            _board[xTo, yTo]?.FallToPos(new Vector2Int(xFrom,yFrom));
            (_board[xFrom, yFrom], _board[xTo, yTo]) = (_board[xTo, yTo], _board[xFrom, yFrom]);
        }
    }

 
}