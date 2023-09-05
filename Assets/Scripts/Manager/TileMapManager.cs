using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class BuildTile
{
    public bool buildable = true;
    public bool isRoad = false;
   
}

public class TileMapManager : MonoBehaviour
{
    public static TileMapManager instance;

    // Start is called before the first frame update
    public Tilemap map;
    BuildTile[,] tiles = new BuildTile[28,18];
    public List<TileData> tileDatas;

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
        if(Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            TileBase clickedTile = map.GetTile(gridPosition);            
        }

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

        return tiles[x, y].isRoad;
    }

   void InitRoad()    
   {
        for (int i = 0; i< 12; i++) 
        {
            tiles[i, 8].isRoad = true;
            tiles[i, 9].isRoad = true;
            tiles[i+14, 8].isRoad = true;
            tiles[i + 14, 9].isRoad = true;

            tiles[i, 7].isRoad = true;
            tiles[i, 10].isRoad = true;
            tiles[i + 14, 7].isRoad = true;
            tiles[i + 14, 10].isRoad = true;
        }

        for (int i = 0; i < 8; i++)
        {
            tiles[12, i].isRoad = true;
            tiles[13, i].isRoad = true;

            tiles[12, i+10].isRoad = true;
            tiles[13, i+10].isRoad = true;

            tiles[11, i].isRoad = true;
            tiles[14, i].isRoad = true;

            tiles[11, i + 10].isRoad = true;
            tiles[14, i + 10].isRoad = true;

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

        if (Mathf.Abs(x1 - x2) < 2)
            isSame = true;
        if (Mathf.Abs(y1 - y2) < 2)
            isSame = true;

        return isSame;
    }
   
}
