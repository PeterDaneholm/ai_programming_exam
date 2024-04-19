using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class corridorNode : NodePCG
{
    private NodePCG structure1;
    private NodePCG structure2;
    private int corridorWidth;

    private int modifierDistanceFromWall = 1;


    public corridorNode(NodePCG nodePCG1, NodePCG nodePCG2, int corridorWidth) : base(null)
    {
        this.structure1 = nodePCG1;
        this.structure2 = nodePCG2;
        this.corridorWidth = corridorWidth;
        GenerateCorridor();
    }

    private void GenerateCorridor()
    {
        var relativePositionOfStructure2 = CheckPositionStructure2AgainstStructure1();
        switch (relativePositionOfStructure2)
        {
            case RelativePosition.Up:
                ProcessRoomInRelationUpOrDown(this.structure1, this.structure2);
                break;
            case RelativePosition.Down:
                ProcessRoomInRelationUpOrDown(this.structure2, this.structure1);
                break;
            case RelativePosition.Left:
                ProcessRoomInRelationLeftOrRight(this.structure2, this.structure1);
                break;
            case RelativePosition.Right:
                ProcessRoomInRelationLeftOrRight(this.structure1, this.structure2);
                break;
        }
    }

    private void ProcessRoomInRelationLeftOrRight(NodePCG structure2, NodePCG structure1)
    {
        NodePCG leftStructure = null;
        List<NodePCG> leftStructureChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure1);
        NodePCG rightStructure = null;
        List<NodePCG> rightStrutureChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure2);

        var sortedLeftStructure = leftStructureChildren.OrderByDescending(ChildrenNodeList => TopRightAreaCorner.x).ToList();
            if(sortedLeftStructure.Count == 1)
            {
                leftStructure = sortedLeftStructure[0];
            } else
            {
                int maxX = sortedLeftStructure[0].TopRightAreaCorner.x;
                sortedLeftStructure = sortedLeftStructure.Where(ChildrenNodeList => Math.Abs(maxX - ChildrenNodeList.TopRightAreaCorner.x) < 10).ToList();
                int index = UnityEngine.Random.Range(0, sortedLeftStructure.Count);
                leftStructure = sortedLeftStructure[index];
            }

            var PossibleNeighboursInRightStructureList = rightStrutureChildren.Where(child => GetValidYForNeighbourLeftRight(
                leftStructure.TopRightAreaCorner, 
                leftStructure.BottomLeftAreaCorner, 
                child.TopRightAreaCorner, 
                child.BottomLeftAreaCorner) 
                != -1).ToList();
                if(PossibleNeighboursInRightStructureList.Count <= 0)
                {
                    rightStructure = structure2;
                }
                else
                {
                    rightStructure = PossibleNeighboursInRightStructureList[0];
                }
                int y = GetValidYForNeighbourLeftRight(
                    leftStructure.TopLeftAreaCorner, leftStructure.BottomRightAreaCorner, 
                    rightStructure.TopLeftAreaCorner, 
                    rightStructure.BottomRightAreaCorner);
                    while(y == -1 && sortedLeftStructure.Count > 1)
                    {
                        sortedLeftStructure = sortedLeftStructure.Where(
                            child => child.TopLeftAreaCorner.y != leftStructure.TopLeftAreaCorner.y).ToList();
                            leftStructure = sortedLeftStructure[0];
                            y = GetValidYForNeighbourLeftRight(
                                leftStructure.TopLeftAreaCorner, leftStructure.BottomRightAreaCorner, 
                                rightStructure.TopLeftAreaCorner, 
                                rightStructure.BottomRightAreaCorner);
                    }
                    BottomLeftAreaCorner = new Vector2Int(leftStructure.BottomRightAreaCorner.x, y);
                    TopRightAreaCorner = new Vector2Int(rightStructure.TopLeftAreaCorner.x, y + corridorWidth);
            
    }

    private int GetValidYForNeighbourLeftRight(Vector2Int leftNodeUp, Vector2Int leftNodeDown, Vector2Int rightNodeUp, Vector2Int rightNodeDown)
    {
        if(rightNodeUp.y >= leftNodeUp.y && leftNodeDown.y >= rightNodeDown.y)
        {
           return StructureHelper.CalculatemiddlePoint(
            leftNodeDown + new Vector2Int(0, modifierDistanceFromWall), 
            leftNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidth)).y;
        }
        if(rightNodeUp.y < leftNodeUp.y && leftNodeDown.y > rightNodeDown.y)
        {
           return StructureHelper.CalculatemiddlePoint(
            rightNodeDown + new Vector2Int(0, modifierDistanceFromWall), 
            rightNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidth)).y;
        }
        if(leftNodeUp.y < rightNodeDown.y && leftNodeUp.y > rightNodeUp.y)
        {
           return StructureHelper.CalculatemiddlePoint(
            rightNodeDown + new Vector2Int(0, modifierDistanceFromWall), 
            leftNodeUp - new Vector2Int(0, modifierDistanceFromWall)).y;
        }
        if(rightNodeDown.y > leftNodeDown.y && leftNodeDown.y < rightNodeUp.y)
        {
            return StructureHelper.CalculatemiddlePoint( 
                leftNodeDown + new Vector2Int( 0, modifierDistanceFromWall), 
                rightNodeUp - new Vector2Int(0, modifierDistanceFromWall + this.corridorWidth)).y;
        }
            return -1;
        
    }

    private void ProcessRoomInRelationUpOrDown(NodePCG structure2, NodePCG structure1)
    {
        NodePCG bottomStructure = null;
        List<NodePCG> structureBottomChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure1);
        NodePCG topStructure = null;
        List<NodePCG> topStructureChildren = StructureHelper.TraverseGraphToExtractLowestLeafes(structure2);

        var sortedBottomStructure = structureBottomChildren.OrderByDescending(child => child.TopRightAreaCorner.y).ToList();
        
        if (sortedBottomStructure.Count == 1)
        {
            bottomStructure = sortedBottomStructure[0];
        }
        else
        {
            int maxY = sortedBottomStructure[0].TopRightAreaCorner.y;
            sortedBottomStructure = sortedBottomStructure.Where(child => Math.Abs(maxY - child.TopRightAreaCorner.y) < 10).ToList();
            int index = UnityEngine.Random.Range(0, sortedBottomStructure.Count);
            bottomStructure = sortedBottomStructure[index];
        }

        var PossibleNeighboursInTopStructure = topStructureChildren.Where(
            child => GetValidXForNeighbourUpDown(
            bottomStructure.TopLeftAreaCorner, 
            bottomStructure.TopRightAreaCorner, 
            child.BottomLeftAreaCorner, 
            child.BottomRightAreaCorner) != -1).ToList();
            if(PossibleNeighboursInTopStructure.Count == 0)
            {
                topStructure = structure2;
            }
            else
            {
                topStructure = PossibleNeighboursInTopStructure[0];
            }
            int x = GetValidXForNeighbourUpDown(
                bottomStructure.TopLeftAreaCorner, 
                bottomStructure.TopRightAreaCorner, 
                topStructure.BottomLeftAreaCorner, 
                topStructure.BottomRightAreaCorner);
                while(x == -1 && sortedBottomStructure.Count > 1)
                {
                    sortedBottomStructure = sortedBottomStructure.Where(
                        child => child.TopLeftAreaCorner.x != bottomStructure.TopLeftAreaCorner.x).ToList();
                        bottomStructure = sortedBottomStructure[0];
                        x = GetValidXForNeighbourUpDown(
                            bottomStructure.TopLeftAreaCorner, 
                            bottomStructure.TopRightAreaCorner, 
                            topStructure.BottomLeftAreaCorner, 
                            topStructure.BottomRightAreaCorner);
                }
                BottomLeftAreaCorner = new Vector2Int(x, bottomStructure.TopLeftAreaCorner.y);
                TopRightAreaCorner = new Vector2Int(x + this.corridorWidth, topStructure.TopLeftAreaCorner.y);
    }

    private int GetValidXForNeighbourUpDown(Vector2Int bottomNodeLeft, Vector2Int bottomNodeRight, Vector2Int TopNodeLeft, Vector2Int TopNodeRight)
    {
        if(TopNodeLeft.x < bottomNodeLeft.x && bottomNodeRight.x < TopNodeRight.x)
        {
           return StructureHelper.CalculatemiddlePoint(
            bottomNodeLeft + new Vector2Int(modifierDistanceFromWall, 0), 
            bottomNodeRight - new Vector2Int(this.corridorWidth+ modifierDistanceFromWall, 0)).x; 
        }
        if(TopNodeLeft.x >= bottomNodeLeft.x && bottomNodeRight.x >= TopNodeRight.x)
        {
          return StructureHelper.CalculatemiddlePoint(
            TopNodeLeft + new Vector2Int(modifierDistanceFromWall, 0), 
            TopNodeRight - new Vector2Int(this.corridorWidth+ modifierDistanceFromWall, 0)).x;  
        }
        if(bottomNodeLeft.x >= (TopNodeLeft.x) && bottomNodeLeft.x <= TopNodeRight.x)
        {
          return StructureHelper.CalculatemiddlePoint(
            bottomNodeLeft + new Vector2Int(modifierDistanceFromWall, 0), 
            TopNodeRight - new Vector2Int(this.corridorWidth+ modifierDistanceFromWall, 0)).x;
        }
        if(bottomNodeRight.x <= TopNodeRight.x && bottomNodeRight.x <= TopNodeLeft.x)
        {
            return StructureHelper.CalculatemiddlePoint(
                TopNodeLeft + new Vector2Int(modifierDistanceFromWall, 0), 
                bottomNodeRight - new Vector2Int(this.corridorWidth+ modifierDistanceFromWall, 0)).x;
        }
        return -1;
        }
     private RelativePosition CheckPositionStructure2AgainstStructure1()
        {
        Vector2 middlePointStructure1Temp = ((Vector2)structure1.TopRightAreaCorner + structure1.BottomLeftAreaCorner) / 2;
        Vector2 middlePointStructure2Temp = ((Vector2)structure2.TopRightAreaCorner + structure2.BottomLeftAreaCorner) / 2;
        float angle = CalculateAngle(middlePointStructure1Temp, middlePointStructure2Temp);
        if((angle < 45 && angle >=0) || (angle > -45 && angle < 0 ))
        {
            return RelativePosition.Right;;
        }
        else if(angle > 45 && angle < 135)
        {
            return RelativePosition.Up;
        }
        else if(angle > 135 || angle < -135)
        {
            return RelativePosition.Down;
        }
        else
        {
            return RelativePosition.Left;
        }

        
        }

        private float CalculateAngle(Vector2 middlePointStructure1Temp, Vector2 middlePointStructure2Temp)
        {
        return Mathf.Atan2(middlePointStructure2Temp.y - middlePointStructure1Temp.y, 
        middlePointStructure2Temp.x - middlePointStructure1Temp.x) * Mathf.Rad2Deg;
        }
}
