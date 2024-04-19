using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength;
    public int roomWidthMin, roomLengthMin;
    public int maxiterations;
    public int corridorWidth;
    // Start is called before the first frame update
    void Start()
    {
        createDungeon();
    }

 private void createDungeon()
    {
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateRooms (maxiterations, roomWidthMin, roomLengthMin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}