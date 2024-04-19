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

   internal object CalculateRooms(int maxiterations, int roomWidthMin, int roomLengthMin)   
   {
        BinarySpacePartitioning bsp = new BinarySpacePartitioning(dungeonWidth, dungeonLength); 
        allSpaceNodes = bsp.PrepareNodesCollection(maxiterations, roomWidthMin, roomLengthMin);
        return new List<Node>(allSpaceNodes);
   }
}
