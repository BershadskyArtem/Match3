using System;
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
        
        private Vector2Int _indexedPosition;
        private NodeBoard _board;
        private IMatchable _matchable;
        private ISwappable _swappable;

        [SerializeField] private TextMeshProUGUI text;
        private void Awake()
        {
            _indexedPosition = new Vector2Int(-1,-1);
            _control.OnSwipe += HandleSwiping;
            _matchable = new MatchProperty(color , isMatchable);
            _swappable = new SwappableProperty(isSwappable);
           
        }

        public void MoveToPosition(Vector2Int index , bool useLongTime)
        {
            if(index == _indexedPosition) return;
            var diff = index.y - _indexedPosition.y;
            _indexedPosition = index;
            
            //Debug.Log(AnimationNumbers.Instance.FallTime * Mathf.Abs(diff));
            if (useLongTime)
            {
                //rect.DOAnchorPos(new Vector2(index.x * AnimationNumbers.Instance.Gap, index.y * AnimationNumbers.Instance.Gap), 
                //    AnimationNumbers.Instance.FallTime * Mathf.Abs(diff) , true).SetEase(AnimationNumbers.Instance.FallCurve); 
                transform.DOLocalMove(new Vector2((float)index.x * AnimationNumbers.Instance.Gap, (float)index.y * AnimationNumbers.Instance.Gap), 
                    AnimationNumbers.Instance.FallTime * Mathf.Abs(diff)).SetEase(AnimationNumbers.Instance.FallCurve); 
                
            }
            else
            {
                //rect.DOAnchorPos(new Vector2(index.x * AnimationNumbers.Instance.Gap, index.y * AnimationNumbers.Instance.Gap), 
                //    AnimationNumbers.Instance.SwapTime , true).SetEase(AnimationNumbers.Instance.SwapCurve);   
                float x = (float) index.x * AnimationNumbers.Instance.Gap;
                float y = (float) index.y * AnimationNumbers.Instance.Gap;
                transform.DOLocalMove(new Vector2(x,y), 
                    AnimationNumbers.Instance.SwapTime).SetEase(AnimationNumbers.Instance.SwapCurve);   
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
            return;
            if (this == null || this.gameObject == null) return;
            
            try
            {
                StartCoroutine(nameof(DestoryEffect));
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            
        }
        
        private IEnumerator DestoryEffect()
        {
           
            if (this.gameObject == null) yield break;
            
            bool s = true;
            bool n = true;
            if (s)
            {
                s = false;
                yield return new WaitForSeconds(0.1f);
            }
            
            var ef = Instantiate(NodeFactory.Instance.GetDestroyPrefab(), rect.parent);
            Vector2 vec = _indexedPosition;
            vec.x *= AnimationNumbers.Instance.Gap;
            vec.y *= AnimationNumbers.Instance.Gap;
            ef.GetComponent<RectTransform>().anchoredPosition = vec;
            this.rect.DOScale(Vector3.zero, AnimationNumbers.Instance.DeathScaleTime).SetEase(AnimationNumbers.Instance.DestroyCurve);

            if (n)
            {
                n = false;
                yield return new WaitForSeconds( AnimationNumbers.Instance.DeathScaleTime + 0.1f);
            }            
            if(this.gameObject != null)
                Destroy(this.gameObject);
           
            yield return null;
        }
        
    }
}
