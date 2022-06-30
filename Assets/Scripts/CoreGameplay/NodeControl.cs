using System;
using CoreGameplay.Kinds;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CoreGameplay
{
    public class NodeControl : MonoBehaviour, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        private float directionalThreshold = 1.5f;
        private bool isBeingSwiped;
        private Vector2 touchStart;
        private Vector2 touchFinish;

        public event Action<Direction> OnSwipe;
        private void HandleSwiping()
        {
            var swpDir = touchFinish - touchStart;
            if (swpDir.sqrMagnitude > 45)
            {
                if (swpDir.x < 0 && Mathf.Abs(swpDir.y) * directionalThreshold < Mathf.Abs(swpDir.x))
                { 
                    Debug.Log("Left");
                    OnSwipe?.Invoke(Direction.Left);
                }
                else if (swpDir.x > 0 && Mathf.Abs(swpDir.y) * directionalThreshold < swpDir.x)
                {
                    Debug.Log("Right");
                    OnSwipe?.Invoke(Direction.Right);
                }
                else if (swpDir.y > 0 && Mathf.Abs(swpDir.x) * directionalThreshold < swpDir.y)
                {
                    Debug.Log("Up");
                    OnSwipe?.Invoke(Direction.Up);
                }
                else if (swpDir.y < 0 && Mathf.Abs(swpDir.x) * directionalThreshold < Mathf.Abs(swpDir.y))
                {
                    Debug.Log("Down");
                    OnSwipe?.Invoke(Direction.Down);
                }
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            isBeingSwiped = true;
            touchStart = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isBeingSwiped)
            {
                touchFinish = eventData.position;
                HandleSwiping();
            }
            isBeingSwiped = false;

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isBeingSwiped)
            {
                touchFinish = eventData.position;
                HandleSwiping();
            }
            isBeingSwiped = false;
        }
    }
}