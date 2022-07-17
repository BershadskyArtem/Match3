using CoreGameplay.Kinds;
using CoreGameplay.Nodes;
using CoreGameplay.Nodes.ColorNodes;
using UnityEngine;


namespace CoreGameplay
{
    public static class NodeFactoryV2
    {
        public static BaseNodeObject GetPrefab(NodeColor color , NodeBoard board , Vector2Int position = new Vector2Int())
        {
            switch (color)
            {
                case NodeColor.Red:
                    return new RedNode(position , board);
                case NodeColor.Green:
                    return new GreenNode(position , board);
                case NodeColor.Yellow:
                    return new YellowNode(position , board);
                case NodeColor.Blue:
                    return new BlueNode(position , board);
                default:
                    return null;
            }
        }

        public static BaseNodeObject GetRandomPrefab(NodeBoard board , Vector2Int position = new Vector2Int())
            => GetPrefab((NodeColor)Random.Range(1 , 5) , board , position);

        public static BaseNodeObject GetOpposite(NodeColor color,NodeBoard board , Vector2Int position = new Vector2Int())
        {
            return color switch
            {
                NodeColor.Red =>        GetPrefab(NodeColor.Blue  , board , position),
                NodeColor.Green =>      GetPrefab(NodeColor.Red   , board , position),
                NodeColor.Yellow =>     GetPrefab(NodeColor.Blue  , board , position),
                NodeColor.Blue =>       GetPrefab(NodeColor.Yellow, board , position),
            };
        }
    }
}