using EnumManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemyPrefab;

    public static EnemyPool instance;

    private Dictionary<MonsterType, List<GameObject>> pooledObjects = new Dictionary<MonsterType, List<GameObject>>();
    int poolingCount = 100;
    private void Awake()
    {
        instance = this;
        CreateMultiplePoolObjects();
    }
    void Start()
    {
        
    }

    public void CreateMultiplePoolObjects()
    {
        for (int i = 0; i < enemyPrefab.Length; i++)
        {
            for (int j = 0; j < poolingCount; j++)
            {
                if (!pooledObjects.ContainsKey(enemyPrefab[i].GetComponent<Enemy>().type))
                {
                    List<GameObject> newList = new List<GameObject>();
                    pooledObjects.Add(enemyPrefab[i].GetComponent<Enemy>().type, newList);
                }

                GameObject enemy = Instantiate(enemyPrefab[i], transform);
                enemy.SetActive(false);
                pooledObjects[enemyPrefab[i].GetComponent<Enemy>().type].Add(enemy);
            }
        }
    }

    public GameObject GetPooledObject(MonsterType type)
    {
        if (pooledObjects.ContainsKey(type))
        {
            for (int i = 0; i < pooledObjects[type].Count; i++)
            {
                if (!pooledObjects[type][i].activeSelf)
                {

                    pooledObjects[type][i].GetComponent<Enemy>().StatusInit(type);
                    pooledObjects[type][i].transform.SetParent(null);
                    pooledObjects[type][i].gameObject.SetActive(true);

                    return pooledObjects[type][i];
                }
            }

            int beforeCreateCount = pooledObjects[type].Count;

            CreateMultiplePoolObjects();
            pooledObjects[type][beforeCreateCount].GetComponent<Enemy>().StatusInit(type);
            pooledObjects[type][beforeCreateCount].transform.SetParent(null);
            pooledObjects[type][beforeCreateCount].gameObject.SetActive(true);

           
            return pooledObjects[type][beforeCreateCount];
        }

        else
        {
            return null;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        pooledObjects[obj.GetComponent<Enemy>().type].Add(obj);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
