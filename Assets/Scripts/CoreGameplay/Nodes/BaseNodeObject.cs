using System;
using CoreGameplay.Kinds;
using CoreGameplay.Nodes.Base;
using UnityEngine;

namespace CoreGameplay.Nodes
{
    public abstract class BaseNodeObject : IMatchable , IMovable , ISwappable , IDestroyable
    {
        protected readonly GameObject GameObject;
        protected readonly RectTransform RectTransform;
        protected readonly Vector2Int IndexedPosition;
        protected readonly NodeBoard Board;
        protected readonly NodeControl NodeControl;


        public event Action<Vector2Int, Direction> OnSwipe;

        public BaseNodeObject(Vector2Int indexedPosition, NodeBoard board)
        {
            IndexedPosition = indexedPosition;
            Board = board;
            GameObject = new GameObject();
            RectTransform = GameObject.AddComponent<RectTransform>();
            NodeControl = GameObject.AddComponent<NodeControl>();
            NodeControl.OnSwipe += HandleControl;
        }

        protected virtual void HandleControl(Direction direction)
        {
            
        }
        
        #region Matchable

        public virtual bool CanMatch() => false;
        
        public virtual int GetId()
        {
            return -1;
        }

        public virtual void Resolve()
        {
            if(!CanMatch()) return;
        }

        #endregion
        
        #region Movable

        public virtual void MoveToPosition(Vector2Int newPosition)
        {
            if (!CanMove()) return;
        }
        public virtual bool CanMove() => false;
        
        public virtual bool CanMoveTo(Vector2Int position) => CanMove();

        #endregion
        
        #region Swappable

        public virtual bool CanSwap() => false;
        
        public virtual bool CanSwapWith(Vector2Int position) => CanSwap();
        
        #endregion

        #region Destroyable

        public virtual void Destroy()
        {
            if (!CanDestroy()) return;
        }

        public virtual bool CanDestroy() => false;
        
        #endregion

    }
}