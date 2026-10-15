using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DungeonGenerator : MonoBehaviour
{

    public class Cell
    {
        public bool Visited = false; //has the algortithm created a room in that slot yet?
        public bool[] status = new bool[4]; //what doors are open in each room?
    }

    [System.Serializable]
    public class rule
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;

        public bool obligatory;

        public int probabiltyOfSpawning(int x, int y)
        {
            // 0 - cannot spawn
            //1 - can spawn
            //2 - has to spawn

            if (x>= minPosition.x && x<maxPosition.x && y >= minPosition.y && y < maxPosition.y)
            {
                if (obligatory)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }
    }

    public Vector2Int size;
    public int startPosition = 0;
    public rule[] rooms;
    public Vector2 offset; //distance between each room
    public List<GameObject> activeRooms;

    QuestManager questManager;
    List<Cell> board;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start");
        activeRooms = new List<GameObject>();
        MazeGenerator();
        
    }


    void GenerateDungeon()
    {
        Debug.Log("Generating Dungeon");
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                int randomRoom = -1; 
                List<int>availableRooms = new List<int>(); 

                for (int k = 0; k < rooms.Length; k++)
                {
                    int p = rooms[k].probabiltyOfSpawning(i, j);
                    if (p == 2) //spawns this certain room in the cell
                    {
                        randomRoom = k;
                        break;
                    }
                    else if (p == 1) //adds to the list of possible rooms to spawn
                    {
                        availableRooms.Add(k);
                    }
                }

                if (randomRoom == -1) //if no room spawned yet
                {
                    if (availableRooms.Count > 0) 
                    { //choose a random room from those available to spawn in the cell
                        randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                    }
                    else
                    {
                        randomRoom = 0; //doesnt spawn a room
                    }
                }

                //Spawns the rooms
                Cell currentCell = board[(i + j * size.x)];
                if (currentCell.Visited)
                {
                    var newRoom = Instantiate(rooms[randomRoom].room,new Vector2(i * offset.x,-j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehavior>();
                    newRoom.UpdateRoom(currentCell.status);//opens the correct doors
                    newRoom.name += " " + i + "-" + j; //gives the rooms a name to show their location on the board
                    
                }
                
            }
        }

        
            
        
    }

    void MazeGenerator()
    {
        Debug.Log("Maze Generator Active");
        board = new List<Cell>();

        //creates the grid that the rooms will generate in
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPosition;

        Stack<int> path = new Stack<int>();

        int k = 0;

        //algorithm that walks around the board to establish the path
        while (k < 1000)//makes sure the dungeon doesn't generate forever
        {
            k++;
            board[currentCell].Visited = true; //a room will spawn here
            if (currentCell == board.Count - 1)
            {
                break;
            }

            //Check the cell's neighbors
            List<int> neighbors = CheckNeighbours(currentCell);

            if (neighbors.Count == 0) //if no neighbours
            { //Wont activate any doors
                if (path.Count == 0)
                {
                    break; 
                }
                else
                {
                    currentCell = path.Pop();  
                }
            }
            else //if the cell has neighbours
            {
                path.Push(currentCell);
              
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    //tells room to open the down or right doors
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    //tells room to open the up or left doors
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }

            }

        }
        GenerateDungeon();
       
    }

    List<int> CheckNeighbours(int cell)
    {
        List<int> neighbors = new List<int>();

        //check up neighbor
        if (cell - size.x >= 0 && !board[(cell - size.x)].Visited)
        {
            neighbors.Add((cell - size.x));
        }

        //check down neighbor
        if (cell + size.x < board.Count && !board[(cell + size.x)].Visited)
        {
            neighbors.Add((cell + size.x));
        }

        //check right neighbor
        if ((cell + 1) % size.x != 0 && !board[(cell + 1)].Visited)
        {
            neighbors.Add((cell + 1));
        }

        //check left neighbor
        if (cell % size.x != 0 && !board[(cell - 1)].Visited)
        {
            neighbors.Add((cell - 1));
        }

        return neighbors;
    }
}
