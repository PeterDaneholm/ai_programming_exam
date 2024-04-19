using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{


    List<RoomNode> allSpaceNodes = new List<RoomNode>();
    private int dungeonWidth;
    private int dungeonLength;


    // Start is called before the first frame update

    public DungeonGenerator(int dungeonWidth, int dungeonLength)
    {
        this.dungeonWidth = dungeonWidth;
        this.dungeonLength = dungeonLength;
    }

    public List<NodePCG> CalculateDungeon(
    int maxiterations, 
    int roomWidthMin,
     int roomLengthMin, 
     float roomBottomCornerModifier, 
     float roomTopCornerModifier, 
     int roomOffset,
     int corridorWidth)
    {
        BinarySpacePartitioning bsp = new BinarySpacePartitioning(dungeonWidth, dungeonLength);
        allSpaceNodes = bsp.PrepareNodesCollection(maxiterations, roomWidthMin, roomLengthMin);
        List<NodePCG> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(bsp.RootNode);
        RoomGenerator roomGenerator = new RoomGenerator(maxiterations, roomLengthMin, roomWidthMin);
        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces, roomBottomCornerModifier, roomTopCornerModifier, roomOffset);
        
        CorridorsGenerator corridorsGenerator = new CorridorsGenerator();
        var corridorList = corridorsGenerator.CreateCorridor(allSpaceNodes, corridorWidth);
        
        return new List<NodePCG>(roomList).Concat(corridorList).ToList();
    }
}
