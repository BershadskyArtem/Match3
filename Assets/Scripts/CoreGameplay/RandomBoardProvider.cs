using System.Linq;
using CoreGameplay.Base;
using CoreGameplay.Matches.Rules;
using CoreGameplay.Nodes;
using UnityEngine;

namespace CoreGameplay
{
    public class RandomBoardProvider : IBoardProvider
    {
        private IMatchDiagnoser<NodeInfo[,]> _matchDiagnoser;

        public RandomBoardProvider(IMatchDiagnoser<NodeInfo[,]> matchDiagnoser)
        {
            _matchDiagnoser = matchDiagnoser;
            _matchDiagnoser
                .AddMatchRule(new InfoCrossMatchRule())
                .AddMatchRule(new InfoHorizontalMatchRule())
                .AddMatchRule(new InfoVerticalMatchRule());
        }

        private NodeInfo[,] GenerateBoard(int width, int height)
        {
            NodeInfo[,] result = new NodeInfo[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (y == height - 1)
                    {
                        result[x, y] = NodeFactory.Instance.GetRandomNodeInfo();
                    }
                    else
                    {
                        result[x, y] = NodeFactory.Instance.GetRandomNodeInfo();
                    }
                }
            }

            return result;
        }

        public GameObject[,] GetNewBoard(int width, int height)
        {
            GameObject[,] result = new GameObject[width, height];

            var infos = GenerateBoard(width, height);

            VerifyInfos(infos);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (y == height - 1)
                    {
                        result[x, y] = NodeFactory.Instance.GetNodeFromInfo(infos[x, y]);
                    }
                    else
                    {
                        result[x, y] = NodeFactory.Instance.GetNodeFromInfo(infos[x, y]);
                    }
                }
            }

            return result;
        }

        private void VerifyInfos(NodeInfo[,] infos)
        {
            int counter = 1;
            int stupid = 0;

            while (counter != 0 && stupid < 50)
            {
                var matches = _matchDiagnoser.GetMatchesFromBoard(infos);
                if (matches.Count() == 0) return;
                stupid++;
                foreach (var match in matches)
                {

                    var center = match.Center;

                    var info = infos[center.x, center.y];
                    var newInfo = NodeFactory.Instance.ToOppositeNodeInfo(info);
                    infos[center.x, center.y] = newInfo;

                    // var prefab = 
                    //     NodeFactory.Instance.GetOpposite(board[match.Center.x, match.Center.y].GetColor());
                    // board[match.Center.x,match.Center.y].DestroyWithNoAnimation();
                    //
                    // Spawn(match.Center.x, match.Center.y, match.Center.x, 15, prefab);

                    //var inst = Instantiate(prefab);
                    //var nodeObject = inst.GetComponent<NodeObject>();
                    //nodeObject.SetBoard(this);
                    //nodeObject.ForcePosition(match.Center);
                    //board[match.Center.x, match.Center.y] = nodeObject;
                }
            }

            if (stupid >= 50)
            {
                Debug.LogError("Stupid Lock");
            }
        }

        public GameObject[,] GetNewBoard()
        {
            throw new System.NotImplementedException();
        }
    }
}