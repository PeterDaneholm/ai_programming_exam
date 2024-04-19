using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StructureHelper
{
    public static List<NodePCG> TraverseGraphToExtractLowestLeafes(NodePCG parentNode)
        {
            Queue<NodePCG> nodesToCheck = new Queue<NodePCG>();
            List<NodePCG> listToReturn = new List<NodePCG>();
            if(parentNode.ChildrenNodeList.Count == 0)
                {
                return new List<NodePCG>(){parentNode};
                }
                foreach (var child in parentNode.ChildrenNodeList)
                {
                    nodesToCheck.Enqueue(child);
                } 
                while (nodesToCheck.Count > 0) 
                {
                    var currentNode = nodesToCheck.Dequeue();
                    if (currentNode.ChildrenNodeList.Count == 0)
                        {
                        listToReturn.Add(currentNode);
                        }
                    else
                        {
                        foreach (var child in currentNode.ChildrenNodeList)
                        {
                            nodesToCheck.Enqueue(child);
                        }
                        }
                }
            return listToReturn;
        }

    public static Vector2Int GenerateBottomLeftCornerBetween(
        Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointmodifier, int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;
        return new Vector2Int(
            Random.Range(minX, (int)(minX + (maxX - minX) * pointmodifier)), 
            Random.Range(minY, (int)(minY + (maxY - minY) * pointmodifier)));

    }

    public static Vector2Int GenerateTopRightCornerBetween(
        Vector2Int boundaryLeftPoint, Vector2Int boundaryRightPoint, float pointmodifier, int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        return new Vector2Int(
            Random.Range((int)(minX + (maxX - minX) * pointmodifier), maxX),
            Random.Range((int)(minY + (maxY - minY) * pointmodifier), maxY));

    }

    public static Vector2Int CalculatemiddlePoint(Vector2Int v1, Vector2Int v2)
    {
        Vector2 sum = v1 + v2;
        Vector2 tempVector = sum / 2;
        return new Vector2Int((int)tempVector.x, (int)tempVector.y);
    }
}


public enum RelativePosition
{
    Left,
    Right,
    Up,
    Down
}