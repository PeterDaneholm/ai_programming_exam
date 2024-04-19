using System.Collections;
using System.Collections.Generic;
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

    public List<NodePCG> CalculateRooms(int maxiterations, int roomWidthMin, int roomLengthMin)
    {
        BinarySpacePartitioning bsp = new BinarySpacePartitioning(dungeonWidth, dungeonLength);
        allSpaceNodes = bsp.PrepareNodesCollection(maxiterations, roomWidthMin, roomLengthMin);
        List<NodePCG> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeafes(bsp.RootNode);
        RoomGenerator roomGenerator = new RoomGenerator(maxiterations, roomLengthMin, roomWidthMin);
        List<RoomNode> rooms = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces);
        return new List<NodePCG>(rooms);
    }
}
