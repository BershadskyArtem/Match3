using UnityEngine;

namespace CoreGameplay
{
    public class NodeGeneratorObject : NodeObject
    {
        [SerializeField] private bool GenerateRandomNodes;
        [SerializeField] private Vector2Int Direction;
        private Vector2Int _position;
        
        public void GenerateNode(NodeBoard board)
        {
            var b = board.GetBoard();
            var pVec = _position + Direction;

            if (board.IsInsideBoard(pVec) && b[pVec.x , pVec.y] == null)
            {
                board.SetNode(pVec,NodeFactory.Instance.GetRandomPrefab());    
            }
        }
        
        public void Init(Vector2Int position)
        {
            _position = position;
        }



    }
}