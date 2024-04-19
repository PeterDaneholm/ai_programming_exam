using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private List<Node> childrenNodeList;
    public  List<Node> ChildrenNodeList{get => childrenNodeList;}

    public bool visited { get; set; }
    public Vector2Int BottomLeftAreaCorner { get; set; }
    public Vector2Int BottomRightAreaCorner { get; set; }
    public Vector2Int TopRightAreaCorner { get; set; }
     public Vector2Int TopLeftAreaCorner { get; set; }

     public int treelayerIndex { get; set; }

    public Node parentNode { get; set; }
     public Node(Node parentNode)
     {
         childrenNodeList = new List<Node>();
         this.parent = parentNode;
         if (parentNode != null)
         {
             parentNode.AddChild(this);
         }
     }

     public void AddChild(Node childNode)
     {
         childrenNodeList.Add(childNode);
     }

        public void RemoveChild(Node childNode)
        {
            childrenNodeList.Remove(childNode);
        }
}
