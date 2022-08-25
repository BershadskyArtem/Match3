using CoreGameplay;
using CoreGameplay.Matches;
using UnityEngine;

namespace CoreNodes
{
    public class GameNode : MonoBehaviour , ISwappableNode
    {
        public bool IsSwappable { get; set; }

        public bool CanSwap(NodeBoard board, GameNode other)
        {
            if (!IsSwappable) return false;
            return false;

        }

        public bool PreSwapReaction(NodeBoard board, GameNode other)
        {
            if (!IsSwappable) return false;
            return true;
        }

        public bool SwapReaction(NodeBoard board, GameNode other)
        {
            if (!IsSwappable) return false;
            return true;
        }

        public bool PostSwapReaction(NodeBoard board, GameNode other)
        {
            if (!IsSwappable) return false;
            return true;
        }
    }
}