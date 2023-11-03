using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
using UnityEngine.Rendering;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Zombie;
    public GameObject FrankStein;

    public GameObject[] spawnPosition;

    float spawnTimer = 0;
    public float maxSpawnTimer = 2f;
    public bool stop = true;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (stop) return;


        if (Input.GetKeyDown(KeyCode.Z))
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject Enemy = Instantiate(FrankStein) as GameObject; 
                Enemy.transform.position = spawnPosition[Random.Range(i*2, i*2+2)].transform.position;
                Enemy.GetComponent<Enemy>().StatusInit(MonsterType.FRANKSTEIN);
            }

            for (int i = 4; i < 8; i++)
            {
                GameObject Enemy = Instantiate(Zombie) as GameObject;
                Enemy.transform.position = spawnPosition[Random.Range(i * 2, i * 2 + 2)].transform.position;
                Enemy.GetComponent<Enemy>().StatusInit(MonsterType.ZOMBIE);
            }
        }


        if (GameManager.instance.isGameTurn && !stop)
        {
            spawnTimer += Time.deltaTime;

            if (GameManager.instance.Stage == 1)
            {

                if (spawnTimer >= maxSpawnTimer)
                {
                    
                     for (int i = 0; i < 4; i++)
                     {
                       
                          GameObject Enemy = Instantiate(Zombie) as GameObject;
                          int spawnNum = Random.Range(i * 2, i * 2 + 2);
                          SetRoadNum(ref Enemy, spawnNum);  
                          Enemy.transform.position = spawnPosition[spawnNum].transform.position;
                          Enemy.GetComponent<Enemy>().StatusInit(MonsterType.ZOMBIE);
                     }

                    spawnTimer = 0;
                }
            }

            if (GameManager.instance.Stage == 2)
            {
                if (spawnTimer >= maxSpawnTimer+0.5f)
                {

                    for (int i = 0; i < 4; i++)
                    {

                        GameObject Enemy = Instantiate(Zombie) as GameObject;
                        int spawnNum = Random.Range(i * 2, i * 2 + 2);
                        SetRoadNum(ref Enemy, spawnNum);
                        Enemy.transform.position = spawnPosition[spawnNum].transform.position;
                        Enemy.GetComponent<Enemy>().StatusInit(MonsterType.ZOMBIE);
                    }

                    for (int i = 4; i < 8; i++)
                    {

                        GameObject Enemy = Instantiate(Zombie) as GameObject;
                        int spawnNum = Random.Range(i * 2, i * 2 + 2);
                        SetRoadNum(ref Enemy, spawnNum);
                        Enemy.transform.position = spawnPosition[spawnNum].transform.position;
                        Enemy.GetComponent<Enemy>().StatusInit(MonsterType.ZOMBIE);
                    }

                    spawnTimer = 0;
                }
            }

            if (GameManager.instance.Stage == 4)
            {
                if (spawnTimer >= maxSpawnTimer)
                {

                    for (int i = 0; i < 4; i++)
                    {

                        GameObject Enemy = Instantiate(FrankStein) as GameObject;
                        int spawnNum = Random.Range(i * 2, i * 2 + 2);
                        SetRoadNum(ref Enemy, spawnNum);
                        Enemy.transform.position = spawnPosition[spawnNum].transform.position;
                        Enemy.GetComponent<Enemy>().StatusInit(MonsterType.FRANKSTEIN);
                    }

                    for (int i = 4; i < 8; i++)
                    {

                        GameObject Enemy = Instantiate(Zombie) as GameObject;
                        int spawnNum = Random.Range(i * 2, i * 2 + 2);
                        SetRoadNum(ref Enemy, spawnNum);
                        Enemy.transform.position = spawnPosition[spawnNum].transform.position;
                        Enemy.GetComponent<Enemy>().StatusInit(MonsterType.ZOMBIE);
                    }

                    for (int i = 0; i < 2; i++)
                    {

                        GameObject Enemy = Instantiate(FrankStein) as GameObject;
                        int spawnNum = Random.Range(0, 16);
                        SetRoadNum(ref Enemy, spawnNum);
                        Enemy.transform.position = spawnPosition[spawnNum].transform.position;
                        Enemy.GetComponent<Enemy>().StatusInit(MonsterType.FRANKSTEIN);
                    }
                    spawnTimer = 0;
                }

            }
        }
       
    }

    void SetRoadNum(ref GameObject e, int rand)
    {
        if (rand == 0 || rand == 1  || rand == 8 || rand ==9)
            e.GetComponent<Enemy>().spawnNum = 2;
        if (rand == 2 || rand == 3  || rand == 10 || rand ==11)
            e.GetComponent<Enemy>().spawnNum = 3;
        if (rand == 4 || rand == 5 || rand == 12 || rand == 13)
            e.GetComponent<Enemy>().spawnNum = 1;
        if (rand ==6 || rand ==7 || rand == 14 || rand == 15)
            e.GetComponent<Enemy>().spawnNum = 4;


    }
}
