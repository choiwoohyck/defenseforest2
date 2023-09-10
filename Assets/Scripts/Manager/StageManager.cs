using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StageManager : MonoBehaviour
{
    [HideInInspector]
    public static StageManager instance;

    public GameObject GlobalLight;
    public float gameTime = 0;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameTurn)
        {
            float color = (GlobalLight.GetComponent<Light2D>().color.r * 255) + Time.deltaTime * 4;
            GlobalLight.GetComponent<Light2D>().color = new Color(color/255f, color / 255f, color / 255f);
            gameTime += Time.deltaTime;
        }

        if (gameTime >= 60)
        {
            GameManager.instance.isGameTurn = false;
            gameTime = 0;
        }
    }

    public void ChangeNight()
    {
        GlobalLight.GetComponent<Light2D>().color = new Color(15/255f, 15/255f, 15/255f);
    }
}
