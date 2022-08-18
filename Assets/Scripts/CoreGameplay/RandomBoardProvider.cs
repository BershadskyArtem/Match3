using CoreGameplay.Base;
using UnityEngine;

namespace CoreGameplay
{
    public class RandomBoardProvider : IBoardProvider
    {
        public GameObject[,] GetNewBoard(int width, int height)
        {
            GameObject[,] result = new GameObject[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (y == height - 1)
                    {
                        result[x, y] = NodeFactory.Instance.GetGenerator();
                    }
                    else
                    {
                        result[x, y] = NodeFactory.Instance.GetRandomPrefab();
                    }
                }
            }
            return result;
        }
        public GameObject[,] GetNewBoard()
        {
            throw new System.NotImplementedException();
        }
    }
}