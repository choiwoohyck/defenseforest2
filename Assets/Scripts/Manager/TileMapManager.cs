using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildTile
{
    public bool buildable = true;
    public bool isRoad = false;
    public int roadNum = 0;
   
}

public class TileMapManager : MonoBehaviour
{
    public static TileMapManager instance;

    // Start is called before the first frame update
    public Tilemap map;
    public BuildTile[,] tiles = new BuildTile[28,18];
    public List<TileData> tileDatas;
    public int playerRoadNum;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < 28; i++)
        {
            for (int j = 0; j < 18; j++)
            {
                tiles[i,j] = new BuildTile();
            }
        }

        InitRoad();
    }

    void Start()
    {
        // 마법돌 주변 설치 못하게

        changeBuildable(13, 9);
        changeBuildable(12, 9);
        changeBuildable(13, 8);
        changeBuildable(12, 8);

    }

    // Update is called once per frame
    void Update()
    {
 
        Vector3 playerPos = GameObject.Find("player").transform.position;
        Vector3Int gridPosition = map.WorldToCell(playerPos);
        int x = gridPosition.x + 13;
        int y = 14 - gridPosition.y;

        playerRoadNum = tiles[x, y].roadNum;
        //Debug.Log(playerRoadNum);
        isPlayerRoad();
    }

    public Vector3 mapPostion()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPosition = map.WorldToCell(mousePosition);


        return gridPosition;
    }

    public bool isBuild()
    {
        int x = (int)TileMapManager.instance.mapPostion().x + 13;
        int y = (int)(14 - TileMapManager.instance.mapPostion().y);

        if (x >= 28 || y >= 18)
            return false;

        return tiles[x, y].buildable;
    }

    public void changeBuildable(int x , int y)
    {
        tiles[x, y].buildable = !tiles[x, y].buildable;
    }

    public bool isRoad()
    {
        int x = (int)TileMapManager.instance.mapPostion().x + 13;
        int y = (int)(14 - TileMapManager.instance.mapPostion().y);

        return tiles[x, y].isRoad;
    }

    public bool isPlayerRoad()
    {
        Vector3 playerPos = GameObject.Find("player").transform.position;
        Vector3Int gridPosition = map.WorldToCell(playerPos);
        int x = gridPosition.x+13;
        int y = 14-gridPosition.y;
        Debug.Log("isplayerRoad " +tiles[x, y].isRoad);
        return tiles[x, y].isRoad;
    }

   void InitRoad()    
   {
        for (int i = 0; i< 12; i++) 
        {
            tiles[i, 8].isRoad = true;
            tiles[i, 9].isRoad = true;
            tiles[i, 7].isRoad = true;
            tiles[i, 10].isRoad = true;


            tiles[i+14, 8].isRoad = true;
            tiles[i + 14, 9].isRoad = true;
            tiles[i + 14, 7].isRoad = true;
            tiles[i + 14, 10].isRoad = true;
        }

        for (int i = 0; i < 8; i++)
        {

            tiles[11, i].isRoad = true;
            tiles[12, i].isRoad = true;
            tiles[13, i].isRoad = true;
            tiles[14, i].isRoad = true;

            tiles[11, i + 10].isRoad = true;
            tiles[12, i+10].isRoad = true;
            tiles[13, i+10].isRoad = true;
            tiles[14, i + 10].isRoad = true;

        }

        for (int i = 0; i < 12; i++)
        {
            for (int j = 7; j < 11; j++)
            {
                tiles[i, j].roadNum = 3;
                tiles[i+14, j].roadNum = 1;
            }
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 11; j < 15; j++)
            {
                tiles[j, i].roadNum = 4;
                tiles[j, i + 10].roadNum = 2;
            }
        }


    }

    public bool isSameRoad(Vector2 pos1, Vector2 pos2)
    {
        
        Vector3Int gridPosition1 = map.WorldToCell(pos1);
        int x1 = gridPosition1.x + 13;
        int y1 = 14 - gridPosition1.y;

        Vector3Int gridPosition2 = map.WorldToCell(pos2);
        int x2 = gridPosition2.x + 13;
        int y2 = 14 - gridPosition2.y;

        bool isSame = false;
        Debug.Log(x1 + ", "  + y1);
        if (tiles[x1, y1].roadNum == tiles[x2, y2].roadNum)
            isSame = true;

        return isSame;
    }


   
}
