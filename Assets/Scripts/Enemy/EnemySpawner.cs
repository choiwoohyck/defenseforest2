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
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
                         int random = Random.Range(0, 2);
                        if (random == 1)
                        {
                            GameObject Enemy = Instantiate(FrankStein) as GameObject;
                            Enemy.transform.position = spawnPosition[Random.Range(i * 2, i * 2 + 2)].transform.position;
                            Enemy.GetComponent<Enemy>().StatusInit(MonsterType.FRANKSTEIN);
                        }
                        else
                        {
                            GameObject Enemy = Instantiate(Zombie) as GameObject;
                            Enemy.transform.position = spawnPosition[Random.Range(i * 2, i * 2 + 2)].transform.position;
                            Enemy.GetComponent<Enemy>().StatusInit(MonsterType.FRANKSTEIN);
                        }
                     }

                    spawnTimer = 0;
                }
            }
        }
       
    }
}
