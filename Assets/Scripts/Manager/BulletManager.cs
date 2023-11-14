using EnumManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] bulletPrefab;
    public static BulletManager instance;
    private Dictionary<OwnerType , List<GameObject>> pooledObjects = new Dictionary<OwnerType, List<GameObject>>();
    int poolingCount = 100;
    // Start is called before the first frame update

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
        for (int i = 0; i < bulletPrefab.Length; i++)
        {
            for (int j = 0; j < poolingCount; j++)
            {
                if (!pooledObjects.ContainsKey(bulletPrefab[i].GetComponent<Bullet>().type))
                {
                    List<GameObject> newList = new List<GameObject>();
                    pooledObjects.Add(bulletPrefab[i].GetComponent<Bullet>().type, newList);
                }

                GameObject bullet = Instantiate(bulletPrefab[i], transform);
                bullet.SetActive(false);
                pooledObjects[bulletPrefab[i].GetComponent<Bullet>().type].Add(bullet);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void GetObject(Vector2 b_startPos, float b_Speed, Vector2 b_rotVec, OwnerType o_type,float damage, GameObject target, GameObject owner = null, bool isDivided = false, int divideNum = 0)
    //{
    //    GameObject returnObj = Instantiate(bulletPrefab[(int)o_type]) as GameObject;
        

    //    returnObj.GetComponent<Bullet>().Init(b_startPos, b_Speed, b_rotVec, o_type,damage,target);
    //    returnObj.transform.SetParent(null);
    //    returnObj.gameObject.SetActive(true);

    //    if (o_type == OwnerType.LEAFELEMENT)
    //        returnObj.GetComponent<Bullet>().SetOwner(owner);

    //}

    public GameObject GetPooledObject(Vector2 b_startPos, float b_Speed, Vector2 b_rotVec, OwnerType o_type, float damage, GameObject target, GameObject owner = null, bool isDivided = false, int divideNum = 0)
    {
        if (pooledObjects.ContainsKey(o_type))
        {
            for (int i = 0; i < pooledObjects[o_type].Count; i++)
            {
                if (!pooledObjects[o_type][i].activeSelf)
                {

                    pooledObjects[o_type][i].GetComponent<Bullet>().Init(b_startPos, b_Speed, b_rotVec, o_type, damage, target,isDivided,divideNum);
                    pooledObjects[o_type][i].transform.SetParent(null);
                    pooledObjects[o_type][i].gameObject.SetActive(true);

                    if (o_type == OwnerType.LEAFELEMENT)
                        pooledObjects[o_type][i].GetComponent<Bullet>().SetOwner(owner);

                    return pooledObjects[o_type][i];
                }
            }

            int beforeCreateCount = pooledObjects[o_type].Count;

            CreateMultiplePoolObjects();
            pooledObjects[o_type][beforeCreateCount].GetComponent<Bullet>().Init(b_startPos, b_Speed, b_rotVec, o_type, damage, target, isDivided, divideNum);
            pooledObjects[o_type][beforeCreateCount].transform.SetParent(null);
            pooledObjects[o_type][beforeCreateCount].gameObject.SetActive(true);

            if (o_type == OwnerType.LEAFELEMENT)
                pooledObjects[o_type][beforeCreateCount].GetComponent<Bullet>().SetOwner(owner);
            return pooledObjects[o_type][beforeCreateCount];
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
        pooledObjects[obj.GetComponent<Bullet>().type].Add(obj);
    }
}
