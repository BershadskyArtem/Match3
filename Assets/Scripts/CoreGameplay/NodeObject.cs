using System;
using System.Collections;
using CoreGameplay.Kinds;
using CoreGameplay.Matches;
using DG.Tweening;
using UnityEngine;

namespace CoreGameplay
{
    [RequireComponent(typeof(RectTransform))]
    public class NodeObject : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private RectTransform rect;
        [SerializeField] private NodeControl _control;
        [SerializeField] private float gap;
        [SerializeField] private float longTime;
        [SerializeField] private float shortTime;
        
        [Header("Node Properties")] 
        [SerializeField] private bool isMatchable;
        [SerializeField] private NodeColor color;
        [SerializeField] private bool isSwappable;
        
        private Vector2Int _indexedPosition;
        private NodeBoard _board;
        private IMatchable _matchable;
        private ISwappable _swappable;

        private void Awake()
        {
            _indexedPosition = new Vector2Int(-100,-100);
            _control.OnSwipe += HandleSwiping;
            _matchable = new MatchProperty(color , isMatchable);
            _swappable = new SwappableProperty(isSwappable);
        }

        public void MoveToPosition(Vector2Int index , bool useLongTime)
        {
            if(index == _indexedPosition) return;
            _indexedPosition = index;
            if (useLongTime)
            {
                rect.DOAnchorPos(new Vector2(index.x * gap, index.y * gap), longTime , true).SetEase(Ease.InQuad);    
            }
            else
            {
                rect.DOAnchorPos(new Vector2(index.x * gap, index.y * gap), shortTime , true).SetEase(Ease.InQuad);
            }
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
            vec.x *= gap;
            vec.y *= gap;
            ef.GetComponent<RectTransform>().anchoredPosition = vec;

            if (n)
            {
                n = false;
                yield return new WaitForSeconds(0.1f);
            }            
            if(this.gameObject != null)
                Destroy(this.gameObject);
           
            yield return null;
        }
        
    }
}
