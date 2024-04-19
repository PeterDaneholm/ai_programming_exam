using System.Collections.Generic;
using UnityEngine;

public abstract class NodePCG
{
    private List<NodePCG> childrenNodeList;
    public List<NodePCG> ChildrenNodeList { get => childrenNodeList; }

    public bool visited { get; set; }
    public Vector2Int BottomLeftAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }
    public Vector2Int TopLeftAreaCorner { get; set; }

    public int treelayerIndex { get; set; }

    public NodePCG Parent { get; set; }
    public NodePCG(NodePCG parentNode)
    {
        childrenNodeList = new List<NodePCG>();
        this.Parent = parentNode;
        if (parentNode != null)
        {
            parentNode.AddChild(this);
        }
    }

    public void AddChild(NodePCG childNode)
    {
        childrenNodeList.Add(childNode);
    }

    public void RemoveChild(NodePCG childNode)
    {
        childrenNodeList.Remove(childNode);
    }
}
