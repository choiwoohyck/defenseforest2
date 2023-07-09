using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using EnumManagerSpace;
using static Bullet;

public class BulletPoolManager: MonoBehaviour
{

    public GameObject[] bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetObject(Vector2 b_startPos, float b_Speed, Vector2 b_rotVec, OwnerType o_type,float damage)
    {
        GameObject returnObj = Instantiate(bulletPrefab[(int)o_type]) as GameObject;
        returnObj.GetComponent<Bullet>().Init(b_startPos, b_Speed, b_rotVec, o_type,damage);
        returnObj.transform.SetParent(null);
        returnObj.gameObject.SetActive(true);
    }

  
}
