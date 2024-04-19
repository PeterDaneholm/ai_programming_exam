using UnityEngine;

public class RoomNode : NodePCG
{
    public RoomNode(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, NodePCG parentNode, int index) : base(parentNode)
    {
        this.BottomLeftAreaCorner = bottomLeftAreaCorner;
        this.TopRightAreaCorner = topRightAreaCorner;
        this.BottomRightAreaCorner = new Vector2Int(topRightAreaCorner.x, bottomLeftAreaCorner.y);
        this.TopLeftAreaCorner = new Vector2Int(bottomLeftAreaCorner.x, topRightAreaCorner.y);
        treelayerIndex = index;
    }

    public int width { get => (int)(TopRightAreaCorner.x - BottomLeftAreaCorner.x); }
    public int length { get => (int)(TopRightAreaCorner.y - BottomLeftAreaCorner.y); }
}
