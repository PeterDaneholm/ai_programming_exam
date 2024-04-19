using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using random = UnityEngine.Random;

public class BinarySpacePartitioning : MonoBehaviour
{
    RoomNode rootNode;
     public RoomNode RootNode { get => rootNode; }
    
    public BinarySpacePartitioning(int dungeonWidth, int dungeonLength)
    {
        this.rootNode = new RoomNode(new Vector2Int(0, 0), new Vector2Int(dungeonWidth, dungeonLength), null, 0);
    }

    public List<RoomNode> PrepareNodesCollection(int maxiterations, int roomWidthMin, int roomLengthMin)
    {
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> listToReturn = new List<RoomNode>();
        graph.Enqueue(this.rootNode);
        listToReturn.Add(this.rootNode);
        int iterations = 0;
        while (iterations < maxiterations && graph.Count >0)
        {
            iterations++;
            RoomNode currentNode = graph.Dequeue();
            if (currentNode.width >=roomWidthMin*2 || currentNode.length >= roomLengthMin*2)
            {
            SplitTheSpace(currentNode, listToReturn, roomLengthMin, roomWidthMin, graph);
            }
        }
        return listToReturn;
    }

    private void SplitTheSpace(RoomNode currentNode, List<RoomNode> listToReturn, int roomLengthMin, int roomWidthMin, Queue<RoomNode> graph)
    {
        Line line = GetLineDividingSpace(
            currentNode.BottomLeftAreaCorner, 
            currentNode.TopRightAreaCorner, 
            roomLengthMin,
             roomWidthMin);

        RoomNode node1, node2;
        if(line.Orientation == Orientation.Horizontal)
        {
            node1 = new RoomNode
            (currentNode.BottomLeftAreaCorner, 
            new Vector2Int(currentNode.TopRightAreaCorner.x, line.Coordinates.y), 
            currentNode, currentNode.treelayerIndex + 1);
            node2 = new RoomNode (new Vector2Int(currentNode.BottomLeftAreaCorner.x, line.Coordinates.y), 
            currentNode.TopRightAreaCorner, 
            currentNode, 
            currentNode.treelayerIndex + 1);
        }
        else
        {
            node1 = new RoomNode (currentNode.BottomLeftAreaCorner, 
            new Vector2Int(line.Coordinates.x, currentNode.TopRightAreaCorner.y), 
            currentNode, currentNode.treelayerIndex + 1);

            node2 = new RoomNode (new Vector2Int(line.Coordinates.x, currentNode.BottomLeftAreaCorner.y), 
            currentNode.TopRightAreaCorner, 
            currentNode, 
            currentNode.treelayerIndex + 1);
        }
        AddNewNodeToCollections(listToReturn, graph, node1);
        AddNewNodeToCollections(listToReturn, graph, node2);
    }

    private void AddNewNodeToCollections(List<RoomNode> listToReturn, Queue<RoomNode> graph, RoomNode node){

        listToReturn.Add(node);
        graph.Enqueue(node);
    }


    private Line GetLineDividingSpace(Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomLengthMin, int roomWidthMin)
    {
        Orientation orientation;
        bool lengthStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) >= 2*roomLengthMin;
        bool widthStatus = (topRightAreaCorner.y - bottomLeftAreaCorner.y) <= 2*roomWidthMin;
        if (lengthStatus && !widthStatus)
        {
        orientation = (Orientation)UnityEngine.Random.Range(0, 2);
        }else if (widthStatus){
            orientation = Orientation.Vertical;
        }
        else
        {
            orientation = Orientation.Horizontal;
        }
        return new Line(orientation, 
        GetCoordinatesFororientation(
        orientation, 
        bottomLeftAreaCorner,
        topRightAreaCorner, 
        roomWidthMin,
        roomLengthMin));
    }

    private Vector2Int GetCoordinatesFororientation(Orientation orientation, Vector2Int bottomLeftAreaCorner, Vector2Int topRightAreaCorner, int roomWidthMin, int roomLengthMin)
    {
        Vector2Int coordinates = Vector2Int.zero;
        if (orientation == Orientation.Horizontal)
        {
            coordinates = new Vector2Int(0, UnityEngine.Random.Range((bottomLeftAreaCorner.y + roomLengthMin), (topRightAreaCorner.y - roomLengthMin)));
        }
        else
        {
            coordinates = new Vector2Int( UnityEngine.Random.Range((bottomLeftAreaCorner.x + roomLengthMin), (topRightAreaCorner.x - roomLengthMin)), 0);
        }
        return coordinates;
    }
}

