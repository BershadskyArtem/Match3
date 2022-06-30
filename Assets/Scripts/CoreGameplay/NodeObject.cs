using System;
using DG.Tweening;
using UnityEngine;

namespace CoreGameplay
{
    [RequireComponent(typeof(RectTransform))]
    public class NodeObject : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private RectTransform rect;
        [SerializeField] private float gap;
        [SerializeField] private float time;

        private Vector2Int _indexedPosition;

        private void Awake()
        {
            _indexedPosition = new Vector2Int(-100,-100);
        }

        public void MoveToPosition(Vector2Int index)
        {
            if(index == _indexedPosition) return;
            _indexedPosition = index;
            rect.DOAnchorPos(new Vector2(index.x * gap, index.y * gap), time , true).SetEase(Ease.InQuad);
        }

    }
}