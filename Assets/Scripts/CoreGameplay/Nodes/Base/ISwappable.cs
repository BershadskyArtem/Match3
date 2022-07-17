using UnityEngine;

namespace CoreGameplay.Nodes.Base
{
    public interface ISwappable
    {
        public bool CanSwap();
        public bool CanSwapWith(Vector2Int position);
    }
}