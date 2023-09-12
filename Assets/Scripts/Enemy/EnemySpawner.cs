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
    public bool stop = false;
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


        if (GameManager.instance.isGameTurn)
        {
            spawnTimer += Time.deltaTime;

            if (GameManager.instance.Stage == 1)
            {
                if (spawnTimer >= maxSpawnTimer)
                {
                    
                     for (int i = 0; i < 4; i++)
                     {
                       
                          GameObject Enemy = Instantiate(Zombie) as GameObject;
                          Enemy.transform.position = spawnPosition[Random.Range(i * 2, i * 2 + 2)].transform.position;
                          Enemy.GetComponent<Enemy>().StatusInit(MonsterType.ZOMBIE);
                     }

                    spawnTimer = 0;
                }
            }

            if (GameManager.instance.Stage == 2)
            {
                if (spawnTimer >= maxSpawnTimer)
                {

                    for (int i = 0; i < 4; i++)
                    {

                        GameObject Enemy = Instantiate(Zombie) as GameObject;
                        Enemy.transform.position = spawnPosition[Random.Range(i * 2, i * 2 + 2)].transform.position;
                        Enemy.GetComponent<Enemy>().StatusInit(MonsterType.ZOMBIE);
                    }

                    for (int i = 4; i < 8; i++)
                    {

                        GameObject Enemy = Instantiate(Zombie) as GameObject;
                        Enemy.transform.position = spawnPosition[Random.Range(i * 2, i * 2 + 2)].transform.position;
                        Enemy.GetComponent<Enemy>().StatusInit(MonsterType.ZOMBIE);
                    }

                    spawnTimer = 0;
                }
            }
        }
       
    }
}
