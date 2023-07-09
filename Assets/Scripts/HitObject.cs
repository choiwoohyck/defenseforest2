using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject : MonoBehaviour
{
    // Start is called before the first frame update

    SpriteRenderer renderer;
    Color originColor;
    bool isChange = false;
    float changeTimer = 0;
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        originColor = renderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isChange)
        {
            changeTimer += Time.deltaTime;
            if (changeTimer >= 0.5f)
            {
                renderer.color = originColor;
                isChange = false;
                changeTimer = 0;
            }
            
        }
    }

    public void ChangeColor()
    {        
        renderer.color = Color.red;
        isChange = true;
    }

  
}
