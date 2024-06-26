using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorsGenerator
{
    public List<NodePCG> CreateCorridor(List<RoomNode> allSpaceNodes, int corridorWidth)
    {
        List<NodePCG> corridorList = new List<NodePCG>();
        Queue<RoomNode> structuresToCheck = new Queue<RoomNode>(
            allSpaceNodes.OrderByDescending (node => node.treelayerIndex).ToList());
        while (structuresToCheck.Count > 0){
            var nodePCG = structuresToCheck.Dequeue();
            if(nodePCG.ChildrenNodeList.Count == 0)
            {
                continue;
            }   
            corridorNode corridor = new corridorNode(nodePCG.ChildrenNodeList[0], nodePCG.ChildrenNodeList[1], corridorWidth);
            corridorList.Add(corridor);

        }
        return corridorList;
    }
}
