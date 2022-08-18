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

        [Header("GeneratorPerfabs")] 
        [SerializeField]private GameObject standartRandomGenerator;

        [Header("Bombs")] 
        [SerializeField] private GameObject Bomb4;
        [SerializeField] private GameObject Bomb5;
        [SerializeField] private GameObject Bomb6;
        
        
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

        public GameObject GetBomb(int rank)
        {
            switch (rank)
            {
                case 4:
                    return Bomb4;
                case 5:
                    return Bomb5;
                case 6:
                    return Bomb6;
                default:
                    return Bomb6;
            }
            
            
        }
        
    }
}