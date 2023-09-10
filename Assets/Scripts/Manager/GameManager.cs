using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public static GameManager instance;

    public bool gameOver = false;
    public bool isGameTurn = false;
    public bool inActiveBuildButton = false;

    public int energy = 1000;

    public int Stage = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        init();
    }

    void init()
    {
        energy = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
