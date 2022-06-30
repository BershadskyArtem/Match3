using UnityEngine;

namespace CoreGameplay.Base
{
    public interface IBoardProvider
    {
        public GameObject[,] GetNewBoard(int width , int height);
        public GameObject[,] GetNewBoard();
    }
}