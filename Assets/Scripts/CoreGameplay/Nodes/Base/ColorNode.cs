using CoreGameplay.Kinds;
using UnityEngine;

namespace CoreGameplay.Nodes.Base
{
    public class ColorNode : BaseNodeObject
    {
        protected NodeColor NodeColor;
        protected GameObject _destroyPrefab;
        public ColorNode(Vector2Int indexedPosition, NodeBoard board, NodeColor nodeColor) : base(indexedPosition, board)
        {
            NodeColor = nodeColor;
        }

        public override bool CanMatch()
        {
            return true;
        }

        public override int GetId()
        {
            return (int) NodeColor;
        }

        public override bool CanDestroy()
        {
            return true;
        }

        public override void Destroy()
        {
            if (_destroyPrefab == null)
            {
                _destroyPrefab = NodeFactory.Instance.GetDestroyPrefab();
            }
            if(!CanDestroy()) return;
            
            



        }
    }
}