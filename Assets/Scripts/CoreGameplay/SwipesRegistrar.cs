using System;
using CoreGameplay.Kinds;
using UnityEngine;

namespace CoreGameplay
{
    public class SwipesRegistrar : MonoBehaviour
    {
        public static event Action<Direction> OnSwipe;
        public static event Action<NodeObject> OnNewNodeSelected;

        private Vector2 swipeStart;
        private Vector2 swipeEnd;
        private Vector2 swipeDelta;

        private float deadZone = 80;
        private bool IsMobile;
        private bool IsSwiping;
        
        private void Start()
        {
            IsMobile = Application.isMobilePlatform;
            OnSwipe?.Invoke(Direction.Down);
        }

        private void SetNewNode()
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll (ray, Mathf.Infinity);
            if (hits.Length == 0)
            {
                OnNewNodeSelected?.Invoke(null);   
                Debug.LogError("Setted as Null");
                return;
            }
            var hit = hits[0];
            var nodeObject = hit.collider.gameObject.GetComponent<NodeObject>();
            OnNewNodeSelected?.Invoke(nodeObject);
            

        }

        private void Update()
        {
            if (IsMobile)
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        IsSwiping = true;
                        swipeStart = Input.GetTouch(0).position;
                        SetNewNode();
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Canceled ||
                             Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        ResetSwipe();
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    IsSwiping = true;
                    swipeStart = Input.mousePosition;
                    SetNewNode();
                }else if (Input.GetMouseButtonUp(0))
                {
                    ResetSwipe();
                }
            }
            
            if(CheckSwipe())
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    OnSwipe?.Invoke(swipeDelta.x > 0 ? Direction.Right : Direction.Left);             
                }
                else
                {
                    OnSwipe?.Invoke(swipeDelta.y > 0 ? Direction.Up : Direction.Down);
                }
                ResetSwipe();
            }
            
        }

        private bool CheckSwipe()
        {
            swipeDelta = Vector2.zero;
            if (IsSwiping)
            {
                if (IsMobile && Input.touchCount > 0)
                {
                    swipeDelta = Input.GetTouch(0).position - swipeStart;
                }
                else if (Input.GetMouseButton(0))
                {
                    swipeDelta = (Vector2) Input.mousePosition - swipeStart;
                }
            }

            return swipeDelta.magnitude > deadZone;


        }

        private void ResetSwipe()
        {
            this.IsSwiping = false;
            swipeStart = Vector2.zero;
            swipeEnd = Vector2.zero;
        }
    }
}