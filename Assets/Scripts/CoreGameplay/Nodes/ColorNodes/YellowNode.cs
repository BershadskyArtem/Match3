using CoreGameplay.Kinds;
using CoreGameplay.Nodes.Base;
using UnityEngine;

namespace CoreGameplay.Nodes.ColorNodes
{
    public class YellowNode : ColorNode
    {
        public YellowNode(Vector2Int indexedPosition, NodeBoard board) : base(indexedPosition, board, NodeColor.Yellow)
        {
        }
    }
}