using UnityEngine;

namespace CoreGameplay.Base
{
    public interface IBoardProvider
    {
        public GameObject[,] GetNewBoard();
    }
}