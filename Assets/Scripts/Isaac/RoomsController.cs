﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ExitSite
{
    up, down, right, left
};

public class RoomsController : MonoBehaviour
{

    static public RoomsController instance;

    public int ammountOfInitialRooms = 10;
    public Room initialRoom;
    public List<Room> allRooms = new List<Room>();
    public List<Room> floorRooms = new List<Room>();
    private List<Room> floorLoaded = new List<Room>();
    private Room currentRoom;

    [SerializeField]
    Room goldRoomPrefab;
    [SerializeField]
    Door doorPrefab;
    [SerializeField]
    Door upDoorPrefab;
    [SerializeField]
    Door goldDoorPrefab;
    [SerializeField]
    Door upGoldDoorPrefab;


    bool isGoldRoomLoaded = false;


    Room newRoom;

    Vector3 startNewRoom;
    Vector3 correction;
    int roomID = 0;


    CameraController cam;
    public GameObject player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {

        Instantiate(player, gameObject.transform);
        cam = Camera.main.GetComponent<CameraController>();

        currentRoom = Instantiate(initialRoom, gameObject.transform);
        floorLoaded.Add(currentRoom);
        currentRoom.SetX(0); currentRoom.setY(0);

        InstantiateAllRooms();
        foreach (Room room in floorLoaded)
        {
            room.SetID(roomID);
            roomID++;
            InstantiateDoors(room);
        }


    }

    public void NewRoom(bool isGoldRoom)
    {
        bool isRoomCreated = false;
        bool isRoomInPosition = false;
        newRoom = null;
        while (!isRoomInPosition)
        {

            //Instantiate a room with a random prefab next to one of the loaded rooms. 

            //Get the instance of a random room already loaded in the level.
            Room randomLoadedRoom = floorLoaded[Random.Range(0, floorLoaded.Count)];

            //0 = up, 1 = down, 2 = right, 3 = left
            int direction = Random.Range(0, 3);

            //Instantiate a room and set the coordinates
            switch (direction)
            {
                case 0:
                    isRoomCreated = InstantiateRoomTop(randomLoadedRoom, isGoldRoom);
                    break;
                case 1:
                    isRoomCreated = InstantiateRoomDown(randomLoadedRoom, isGoldRoom);
                    break;
                case 2:
                    isRoomCreated = InstantiateRoomRight(randomLoadedRoom, isGoldRoom);
                    break;
                case 3:
                    isRoomCreated = InstantiateRoomLeft(randomLoadedRoom, isGoldRoom);
                    break;
            }

            //If the new room is created, set its position.
            if (isRoomCreated)
            {
                newRoom.transform.position = correction;
                currentRoom = newRoom;

                floorLoaded.Add(currentRoom);
                isRoomInPosition = true;
                Debug.Log(currentRoom.transform.position + " currentRoomTransformPos");
            }
        }
    }

    #region Methods for checking coordinates up, down, right and left.

    /// <summary>
    /// Checks if the the coordinate in the top of the base room is free.
    /// If is free, instantiate a random room, set the vector correction and return true.
    /// Else, return false.
    /// </summary>

    /// <param name="baseRoom"> The base room </param>
    /// <param name="isGoldRoom"> If true, the room will be gold. </param>
    /// <returns></returns>
    bool InstantiateRoomTop(Room baseRoom, bool isGoldRoom)
    {
        //Get the base room coordinates
        int baseRoomX = baseRoom.getX();
        int baseRoomY = baseRoom.getY();

        //Create the new room coordinates.
        int newRoomX = baseRoomX;
        int newRoomY = baseRoomY + 1;

        if (!CheckCoordinate(newRoomX, newRoomY)[0])
        {
            if (isGoldRoom)
            {
                newRoom = InstantiateRoom(true);
            }
            else
                newRoom = InstantiateRoom(false);

            //Set the coordinates of the new room.
            setNewRoomXY(newRoom, newRoomX, newRoomY);


            startNewRoom = baseRoom.roomSpawnTop.transform.position;
            correction = new Vector3(
            startNewRoom.x - newRoom.roomSpawnDown.transform.position.x,
            startNewRoom.y - newRoom.roomSpawnDown.transform.position.y, 0);
            return true;
        }
        else
            return false;

    }
    /// <summary>
    /// Checks if the the coordinate in the bottom side of the base room is free.
    /// If is free, instantiate a random room, set the vector correction and return true.
    /// Else, return false.
    /// </summary>

    /// <param name="baseRoom"> The base room </param>
    /// <param name="isGoldRoom"> If true, the room will be gold. </param>
    /// <returns></returns>
    bool InstantiateRoomDown(Room baseRoom, bool isGoldRoom)
    {
        //Get the base room coordinates
        int baseRoomX = baseRoom.getX();
        int baseRoomY = baseRoom.getY();

        //Create the new room coordinates.
        int newRoomX = baseRoomX;
        int newRoomY = baseRoomY - 1;

        if (!CheckCoordinate(newRoomX, newRoomY)[0])
        {
            if (isGoldRoom)
            {
                newRoom = InstantiateRoom(true);
            }
            else
                newRoom = InstantiateRoom(false);

            //Set the coordinates of the new room.
            setNewRoomXY(newRoom, newRoomX, newRoomY);

            //Correction is the vector were we will move our new room.
            startNewRoom = baseRoom.roomSpawnDown.transform.position;
            correction = new Vector3(
            startNewRoom.x - newRoom.roomSpawnTop.transform.position.x,
            startNewRoom.y - newRoom.roomSpawnTop.transform.position.y, 0);
            return true;
        }
        else
            return false;

    }
    /// <summary>
    /// Checks if the the coordinate in the right of the base room is free.
    /// If is free, instantiate a random room, set the vector correction and return true.
    /// Else, return false.
    /// </summary>

    /// <param name="baseRoom"> The base room </param>
    /// <param name="isGoldRoom"> If true, the room will be gold. </param>
    /// <returns></returns>
    bool InstantiateRoomRight(Room baseRoom, bool isGoldRoom)
    {
        //Get the base room coordinates
        int baseRoomX = baseRoom.getX();
        int baseRoomY = baseRoom.getY();

        //Create the new room coordinates.
        int newRoomX = baseRoomX + 1;
        int newRoomY = baseRoomY;

        if (!CheckCoordinate(newRoomX, newRoomY)[0])
        {
            if (isGoldRoom)
            {
                newRoom = InstantiateRoom(true);
            }
            else
                newRoom = InstantiateRoom(false);

            //Set the coordinates of the new room.
            setNewRoomXY(newRoom, newRoomX, newRoomY);

            //Correction is the vector were we will move our new room.
            startNewRoom = baseRoom.roomSpawnRight.transform.position;
            correction = new Vector3(
            startNewRoom.x - newRoom.roomSpawnLeft.transform.position.x,
            startNewRoom.y - newRoom.roomSpawnLeft.transform.position.y, 0);
            return true;
        }
        else
            return false;

    }
    /// <summary>
    /// Checks if the the coordinate in the left of the base room is free.
    /// If is free, instantiate a random room, set the vector correction and return true.
    /// Else, return false.
    /// </summary>

    /// <param name="baseRoom"> The base room </param>
    /// <param name="isGoldRoom"> If true, the room will be gold. </param>
    /// <returns></returns>
    bool InstantiateRoomLeft(Room baseRoom, bool isGoldRoom)
    {
        //Get the base room coordinates
        int baseRoomX = baseRoom.getX();
        int baseRoomY = baseRoom.getY();

        //Create the new room coordinates.
        int newRoomX = baseRoomX - 1;
        int newRoomY = baseRoomY;

        if (!CheckCoordinate(newRoomX, newRoomY)[0])
        {
            if (isGoldRoom)
            {
                newRoom = InstantiateRoom(true);
            }
            else
                newRoom = InstantiateRoom(false);

            //Set the coordinates of the new room.
            setNewRoomXY(newRoom, newRoomX, newRoomY);

            //Correction is the vector were we will move our new room.
            startNewRoom = baseRoom.roomSpawnLeft.transform.position;
            correction = new Vector3(
            startNewRoom.x - newRoom.roomSpawnRight.transform.position.x,
            startNewRoom.y - newRoom.roomSpawnRight.transform.position.y, 0);
            return true;

        }
        else
            return false;

    }

    #endregion

    /// <summary>
    /// Instantiate a random room from the allRooms list.
    /// </summary>
    /// <param name="isGoldRoom"> If true the room will be gold </param>
    /// <returns> An instance of the new room </returns>
    Room InstantiateRoom(bool isGoldRoom)
    {

        bool roomLoaded = false;
        while (roomLoaded == false)
        {
            Room roomToInstantiate;

            if (isGoldRoom)
            {
                roomToInstantiate = goldRoomPrefab;
            }
            else { roomToInstantiate = allRooms[Random.Range(0, allRooms.Count)]; }

            //There should be only one gold room per level.
            if (roomToInstantiate.isGold == true)
            {
                //If the gold room is been not loaded yet, instantiate the gold room. 
                //Else restart the loop.
                if (isGoldRoomLoaded == false)
                {
                    isGoldRoomLoaded = true;
                    roomLoaded = true;
                    return Instantiate(roomToInstantiate);
                }
            }
            else
            {
                roomLoaded = true;
                return Instantiate(roomToInstantiate);
            }

        }
        return null;

    }
    /// <summary>
    ///Checks if the given coordinate is taken and tells if it is a gold room.
    /// </summary>
    /// <param name="newRoomX"></param>
    /// <param name="newRoomY"></param>
    /// <returns> Returns an array of 2 bools. 
    /// First one is true if there is a room in the given coordinate.
    ///  Second one is true if it's a gold room.
    /// </returns>
    bool[] CheckCoordinate(int newRoomX, int newRoomY)
    {
        bool coordinateTaken = false;
        bool isGold = false;
        foreach (Room room in floorLoaded)
        {

            if (room.getX() == newRoomX && room.getY() == newRoomY)
            {
                coordinateTaken = true;
                if (room.isGold)
                {
                    isGold = true;
                }


            }
        }
        bool[] values = { coordinateTaken, isGold };
        return values;
    }

    /// <summary>
    /// Check if there are rooms around the current room and instantiate a door per room.
    /// </summary>
    /// <param name="room"> The current room </param>
    void InstantiateDoors(Room room)
    {
        bool[] values;

        //If there's a room in the right side of the current room instantiate a door
        values = CheckCoordinate(room.x + 1, room.y);
        if (values[0])
        {
            InstantiateRightDoor(room, values[1]);
        }

        //If there's a room in the left side of the current room instantiate a door
        values = CheckCoordinate(room.x - 1, room.y);
        if (values[0])
        {
            InstantiateLeftDoor(room, values[1]);
        }

        //If there's a room in the top side of the current room instantiate a door
        values = CheckCoordinate(room.x, room.y + 1);
        if (values[0])
        {
            InstantiateTopDoor(room, values[1]);
        }

        //If there's a room in the down side of the current room instantiate a door
        values = CheckCoordinate(room.x, room.y - 1);
        if (values[0])
        {
            InstantiateDownDoor(room, values[1]);
        }
    }

    #region Instantiate each door methods
    void InstantiateRightDoor(Room room, bool isGoldRoom)
    {
        GameObject doorPosition = room.doorRightPos;

        //Disable the collider used as a substitute of the door. 
        doorPosition.GetComponent<BoxCollider2D>().enabled = false;

        //If the door is in a side of the room, rotates the door prefab 90 degrees.
        Quaternion doorAngle = Quaternion.AngleAxis(90f, Vector3.forward);

        //If the room is a gold one, instantiate a goldDoor prefab, else instantiate a normal door prefab. 
        if (isGoldRoom) { Instantiate(goldDoorPrefab, doorPosition.transform.position, doorAngle, room.transform); }
        else
        {
            Instantiate(doorPrefab, doorPosition.transform.position, doorAngle, room.transform);
        }
    }
    void InstantiateLeftDoor(Room room, bool isGoldRoom)
    {
        GameObject doorPosition = room.doorLeftPos;

        //Disable the collider used as a substitute of the door. 
        doorPosition.GetComponent<BoxCollider2D>().enabled = false;

        //If the door is in a side of the room, rotates the door prefab 90 degrees.
        Quaternion doorAngle = Quaternion.AngleAxis(90f, Vector3.forward);
        if (isGoldRoom) { Instantiate(goldDoorPrefab, doorPosition.transform.position, doorAngle, room.transform); }
        else
        {
            Instantiate(doorPrefab, doorPosition.transform.position, doorAngle, room.transform);
        }
    }
    void InstantiateTopDoor(Room room, bool isGoldRoom)
    {
        GameObject doorPosition = room.doorUpPos;

        //Disable the collider used as a substitute of the door. 
        doorPosition.GetComponent<BoxCollider2D>().enabled = false;

        //If the door is in a side of the room, rotates the door prefab 90 degrees.
        Quaternion doorAngle = Quaternion.identity;
        if (isGoldRoom) { Instantiate(upGoldDoorPrefab, doorPosition.transform.position, doorAngle, room.transform); }
        else
        {
            Instantiate(upDoorPrefab, doorPosition.transform.position, doorAngle, room.transform);
        }
    }
    void InstantiateDownDoor(Room room, bool isGoldRoom)
    {
        GameObject doorPosition = room.doorDownPos;

        //Disable the collider used as a substitute of the door. 
        doorPosition.GetComponent<BoxCollider2D>().enabled = false;

        //If the door is in a side of the room, rotates the door prefab 90 degrees.
        Quaternion doorAngle = Quaternion.identity;
        if (isGoldRoom) { Instantiate(goldDoorPrefab, doorPosition.transform.position, doorAngle, room.transform); }
        else
        {
            Instantiate(doorPrefab, doorPosition.transform.position, doorAngle, room.transform);
        }
    }

    #endregion

    void InstantiateAllRooms()
    {

        for (int i = 0; i < ammountOfInitialRooms; i++)
        {
            NewRoom(false);

        }

        if (!isGoldRoomLoaded)
        {
            NewRoom(true);
        }

    }

    public void setNewRoomXY(Room room, int valueX, int valueY)
    {
        room.SetX(valueX); room.setY(valueY);
    }


}
