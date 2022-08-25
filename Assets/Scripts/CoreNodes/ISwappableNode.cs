using CoreGameplay;

namespace CoreNodes
{
    public interface ISwappableNode
    {
        public bool IsSwappable { get; set; }
        public bool CanSwap(NodeBoard board, GameNode other);
        public bool PreSwapReaction(NodeBoard board, GameNode other);
        public bool SwapReaction(NodeBoard board, GameNode other);
        public bool PostSwapReaction(NodeBoard board, GameNode other);
    }
}