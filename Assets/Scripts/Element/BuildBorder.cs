using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBorder : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite buildableSprite;
    public Sprite cantBuildableSprite;
    public Sprite isRoadSprite;

    SpriteRenderer renderer;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = TileMapManager.instance.mapPostion() + new Vector3(0.5f, 0.5f);

        if (TileMapManager.instance.isBuild())
        {
            renderer.sprite = buildableSprite;
        }

        else
        {
            renderer.sprite = cantBuildableSprite;
        }

        if (TileMapManager.instance.isRoad())
        {
            renderer.sprite = isRoadSprite;
        }

        

    }
}
