using CoreGameplay.Kinds;
using CoreGameplay.Nodes.Base;
using UnityEngine;

namespace CoreGameplay.Nodes.ColorNodes
{
    public class RedNode : ColorNode
    {
        public RedNode(Vector2Int indexedPosition, NodeBoard board) : base(indexedPosition, board, NodeColor.Red)
        {
        }
    }
}