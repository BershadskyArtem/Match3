using System;
using CoreGameplay.Kinds;
using CoreGameplay.Nodes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CoreGameplay
{
    public class NodeFactory : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject redNodePrefab;
        [SerializeField] private GameObject greenNodePrefab;
        [SerializeField] private GameObject yellowNodePrefab;
        [SerializeField] private GameObject blueNodePrefab;
        [SerializeField] private GameObject unknownNodePrefab;

        [Header("Effects prefabs")] 
        [SerializeField] private GameObject destroyPrefab;

        [Header("GeneratorPerfabs")] 
        [SerializeField]private GameObject standartRandomGenerator;

        [Header("Bombs")] 
        [SerializeField] private GameObject VerticalBomb;
        [SerializeField] private GameObject HorizontalBomb;
        [SerializeField] private GameObject BiggestBomb;
        [SerializeField] private GameObject Bomb;
        
        
        public GameObject GetPrefab(NodeColor color)
        {
            switch (color)
            {
                case NodeColor.Red:
                    return redNodePrefab;
                case NodeColor.Green:
                    return greenNodePrefab;
                case NodeColor.Yellow:
                    return yellowNodePrefab;
                case NodeColor.Blue:
                    return blueNodePrefab;
                default:
                    return unknownNodePrefab;
            }
        }

        public GameObject GetRandomPrefab() => GetPrefab((NodeColor)Random.Range(1 , 5));

        public GameObject GetOpposite(NodeColor color)
        {
            return color switch
            {
                NodeColor.Red => GetPrefab(NodeColor.Blue),
                NodeColor.Green => GetPrefab(NodeColor.Red),
                NodeColor.Yellow => GetPrefab(NodeColor.Blue),
                NodeColor.Blue => GetPrefab(NodeColor.Yellow),
                _ => unknownNodePrefab
            };
        }

        public GameObject GetDestroyPrefab()
        {
            return destroyPrefab;
        }

        public GameObject GetGenerator()
        {
            return standartRandomGenerator;
        }
        
        public static NodeFactory Instance;

        private void Awake()
        {
            Instance = this;
        }

        public GameObject GetBomb(BombKind kind)
        {
            switch (kind)
            {
                case BombKind.Color:
                {
                    return BiggestBomb;
                }
                case BombKind.Bomb:
                {
                    return Bomb;
                }
                case BombKind.Horizontal:
                {
                    return HorizontalBomb;
                }
                case BombKind.Vertical:
                {
                    return VerticalBomb;
                }
            }
            return null;
        }

        public GameObject GetNodeFromInfo(NodeInfo info)
        {
            return GetPrefab(info.Color);
        }

        public NodeInfo ToOppositeNodeInfo(NodeInfo info)
        {
            var result = GetRandomNodeInfo();

            switch (info.Color)
            {
                case NodeColor.Red:
                    result.Color = NodeColor.Blue;
                    break;
                case NodeColor.Green:
                    result.Color = NodeColor.Yellow;
                    break;
                case NodeColor.Yellow:
                    result.Color = NodeColor.Red;
                    break;
                case NodeColor.Blue:
                    result.Color = NodeColor.Green;
                    break;
                case NodeColor.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;

        }
        
        public NodeInfo GetRandomNodeInfo()
        {
            return new NodeInfo()
            {
                IsBomb = false,
                BombKind = BombKind.Bomb,
                Color = (NodeColor)Random.Range(1, 5),
                IsMatchable = true,
                IsSwappable = true
            };
        }
        
    }
}