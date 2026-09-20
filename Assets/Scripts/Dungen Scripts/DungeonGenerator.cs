using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine.UIElements;

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

    QuestManager questManager;
    List<Cell> board;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Start");
        MazeGenerator();
        
    }

    public void Spawner()
    {
        
        foreach (Quest q in questManager.activeQuests)
        {
            
        }
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
                    if (p == 2)
                    {
                        randomRoom = k;
                        break;
                    }
                    else if (p == 1)
                    {
                        availableRooms.Add(k);
                    }
                }

                if (randomRoom == -1)
                {
                    if (availableRooms.Count > 0)
                    {
                        randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                    }
                    else
                    {
                        randomRoom = 0;
                    }
                }


                Cell currentCell = board[(i + j * size.x)];
                if (currentCell.Visited)
                {
                    var newRoom = Instantiate(rooms[randomRoom].room,new Vector2(i * offset.x,-j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehavior>();
                    newRoom.UpdateRoom(currentCell.status);
                    newRoom.name += " " + i + "-" + j;
                }
            }
        }
    }

    void MazeGenerator()
    {
        Debug.Log("Maze Generator Active");
        board = new List<Cell>();

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

        while (k < 1000)
        {
            k++;

            board[currentCell].Visited = true;

           

            if (currentCell == board.Count - 1)
            {
                break;
            }

            //Check the cell's neighbors
            List<int> neighbors = CheckNeighbours(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                    
                }
            }
            else
            {
                path.Push(currentCell);
              
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    //down or right
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
                    //up or left
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
