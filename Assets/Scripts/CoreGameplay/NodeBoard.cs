using CoreGameplay.Base;
using UnityEngine;

namespace CoreGameplay
{
    public class NodeBoard : MonoBehaviour
    {
        private NodeObject[,] _board;
        private readonly IBoardProvider _boardProvider;
        
        public NodeBoard()
        {
            _boardProvider = new RandomBoardProvider();
        }
    }
}