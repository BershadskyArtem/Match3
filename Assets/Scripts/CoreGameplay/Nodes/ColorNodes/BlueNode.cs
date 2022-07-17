using CoreGameplay.Kinds;
using CoreGameplay.Nodes.Base;
using UnityEngine;

namespace CoreGameplay.Nodes.ColorNodes
{
    public class BlueNode : ColorNode
    {
        public BlueNode(Vector2Int indexedPosition, NodeBoard board) : base(indexedPosition, board, NodeColor.Blue)
        {
        }
    }
}