using System.Collections;
using CoreGameplay.Kinds;
using CoreGameplay.Matches;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CoreGameplay
{
    //[RequireComponent(typeof(RectTransform))]
    public class NodeObject : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private RectTransform rect;
        [SerializeField] private NodeControl _control;
        [SerializeField] private Transform transform;
        
        [Header("Node Properties")] 
        [SerializeField] private bool isMatchable;
        [SerializeField] private NodeColor color;
        [SerializeField] private bool isSwappable;
        [SerializeField] private bool isBomb;
        [SerializeField] private int bombRank;
        
        public Vector2Int _indexedPosition;
        private NodeBoard _board;
        private IMatchable _matchable;
        private ISwappable _swappable;


        private bool _isMoving;

        [SerializeField] private TextMeshProUGUI text;
        public Vector2Int IndexedPosition
        {
            get => _indexedPosition;
        }

        public bool IsBomb => isBomb;
        public int BombRank => bombRank;

        private void Awake()
        {
            _indexedPosition = new Vector2Int(-1,-1);
            _control.OnSwipe += HandleSwiping;
            _matchable = new MatchProperty(color , isMatchable);
            _swappable = new SwappableProperty(isSwappable);
           
        }
        
        public void MoveToPositionLerp2(Vector2Int index , bool useLongTime)
        {
            if(index == _indexedPosition) return;
            var diff = index.y - _indexedPosition.y;
            _indexedPosition = index;
            var destination = new Vector2((float) index.x * AnimationNumbers.NodeGap,
                (float) index.y * AnimationNumbers.NodeGap);
            if (useLongTime)
            {
                StartCoroutine(MoveCoroutine(destination, AnimationNumbers.FallSpeed));
            }
            else
            {
                StartCoroutine(MoveCoroutine(destination, AnimationNumbers.SwapSpeed));
            }
        }

        public IEnumerator MoveCoroutine(Vector2 destination , float time)
        {
            Vector3 start = this.gameObject.transform.localPosition;
            Vector3 end = destination;
            Debug.Log("Started moving");
            for (float i = 0; i <= 1 * time; i+= Time.deltaTime)
            {
                this.gameObject.transform.localPosition = Vector3.Slerp(start, end, i / time);
                yield return null;
            }
            this.gameObject.transform.localPosition = destination;
        }

        public void ForcePosition(Vector2Int destination)
        {
            transform.localPosition = new Vector2((float) destination.x * AnimationNumbers.NodeGap,
                (float) destination.y * AnimationNumbers.NodeGap);
        }


        public void FallToPos(Vector2Int destination)
        {
            _indexedPosition = destination;
            transform.DOLocalMove(new Vector2((float)destination.x * AnimationNumbers.NodeGap, (float)destination.y * AnimationNumbers.NodeGap), 
                AnimationNumbers.FallSpeed).SetEase(AnimationNumbers.FallMovCurve); 
        }

        public void SwapToPos(Vector2Int destination)
        {
            _indexedPosition = destination;
            transform.DOLocalMove(new Vector2((float)destination.x * AnimationNumbers.NodeGap, (float)destination.y * AnimationNumbers.NodeGap), 
                AnimationNumbers.SwapSpeed).SetEase(AnimationNumbers.SwapMovCurve);
        }

        public void MoveToPositionLerp(Vector2Int index , bool useLongTime)
        {
            //if(index == _indexedPosition) return;
            var diff = index.y - _indexedPosition.y;
            _indexedPosition = index;
            if (useLongTime)
            {
                //rect.DOAnchorPos(new Vector2(index.x * AnimationNumbers.Instance.Gap, index.y * AnimationNumbers.Instance.Gap), 
                //    AnimationNumbers.Instance.FallTime * Mathf.Abs(diff) , true).SetEase(AnimationNumbers.Instance.FallCurve); 
                transform.DOLocalMove(new Vector2((float)index.x * AnimationNumbers.NodeGap, (float)index.y * AnimationNumbers.NodeGap), 
                    AnimationNumbers.FallSpeed).SetEase(AnimationNumbers.FallMovCurve); 
                
            }
            else
            {
                //rect.DOAnchorPos(new Vector2(index.x * AnimationNumbers.Instance.Gap, index.y * AnimationNumbers.Instance.Gap), 
                //    AnimationNumbers.Instance.SwapTime , true).SetEase(AnimationNumbers.Instance.SwapCurve);   
                float x = (float) index.x * AnimationNumbers.NodeGap;
                float y = (float) index.y * AnimationNumbers.NodeGap;
                transform.DOLocalMove(new Vector2(x,y), 
                    AnimationNumbers.SwapSpeed).SetEase(AnimationNumbers.SwapMovCurve);   
            }

            //text.text = $"{_indexedPosition.x}:{_indexedPosition.y}";
        }

        public ISwappable GetSwappable() => _swappable;
        public IMatchable GetMatchable() => _matchable;

        public void SetBoard(NodeBoard board)
        {
            _board = board;
        }

        public NodeColor GetColor() => color;

        private void HandleSwiping(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    _board.TrySwipeTwoNodes(_indexedPosition , _indexedPosition + Vector2Int.down);
                    break;
                case Direction.Left:
                    _board.TrySwipeTwoNodes(_indexedPosition , _indexedPosition + Vector2Int.left);
                    break;
                case Direction.Right:
                    _board.TrySwipeTwoNodes(_indexedPosition , _indexedPosition + Vector2Int.right);
                    break;
                case Direction.Up:
                    _board.TrySwipeTwoNodes(_indexedPosition , _indexedPosition + Vector2Int.up);
                    break;
            }
        }

        public void DestroyNode()
        {
            //gameObject.transform.DOScale(Vector3.zero, AnimationNumbers.DeathAnimationSpeed).SetEase(AnimationNumbers.DestroyScaleCurve);
            gameObject.transform.DOLocalMove(new Vector3(-4,4.22f,0), AnimationNumbers.NodeFlyAwayTime)    
                .onComplete += () =>
            {
                Destroy(gameObject);
            }; 
            Destroy(this); 
        }

        public void SpawnFrom(Vector2Int spawnPoint , Vector2Int index)
        {
            transform.localPosition = (Vector2)spawnPoint;
            _indexedPosition = index;
            transform.DOLocalMove(
                new Vector2((float) index.x * AnimationNumbers.NodeGap, (float) index.y * AnimationNumbers.NodeGap),
                AnimationNumbers.FallSpeed).SetEase(AnimationNumbers.FallMovCurve).onComplete = () =>
            {
                Debug.Log($"Piece {index} moved to {transform.localPosition}");
            };

        }
        
        
    }
}
