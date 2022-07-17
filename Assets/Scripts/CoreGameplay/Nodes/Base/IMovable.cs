using UnityEngine;

namespace CoreGameplay.Nodes.Base
{
    public interface IMovable
    {
        public void MoveToPosition(Vector2Int newPosition);
        public bool CanMoveTo(Vector2Int position);
        public bool CanMove();
    }
}