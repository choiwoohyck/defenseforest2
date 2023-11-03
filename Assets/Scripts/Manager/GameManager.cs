using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public static GameManager instance;

    public EnemySpawner spawner;

    public bool gameOver = false;
    public bool isGameTurn = false;
    public bool inActiveBuildButton = false;

    public int energy = 800;

    public int Stage = 0;

    public int gameMaxTime = 60;

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
        energy = 800;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
