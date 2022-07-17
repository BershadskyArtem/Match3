using CoreGameplay.Kinds;
using CoreGameplay.Nodes.Base;
using UnityEngine;

namespace CoreGameplay.Nodes.ColorNodes
{
    public class GreenNode : ColorNode
    {
        public GreenNode(Vector2Int indexedPosition, NodeBoard board) : base(indexedPosition, board, NodeColor.Green)
        {
        }
    }
}