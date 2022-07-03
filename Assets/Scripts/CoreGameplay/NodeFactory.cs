using System;
using CoreGameplay.Kinds;
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
        
        public static NodeFactory Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}