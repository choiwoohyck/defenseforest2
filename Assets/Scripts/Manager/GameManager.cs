using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public bool middleBossKillFail = false;
    public bool finalBossKillFail = false;


    public int energy = 800;
    public int enemyCnt = 0;
    public int Stage = 0;

    public int gameMaxTime = 60;

    public bool isTutorialFinished = false;

    
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
