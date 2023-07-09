using EnumManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] bulletPrefab;
    public static BulletManager instance;


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

    }

    public void GetObject(Vector2 b_startPos, float b_Speed, Vector2 b_rotVec, OwnerType o_type,float damage)
    {
        GameObject returnObj = Instantiate(bulletPrefab[(int)o_type]) as GameObject;
        returnObj.GetComponent<Bullet>().Init(b_startPos, b_Speed, b_rotVec, o_type,damage);
        returnObj.transform.SetParent(null);
        returnObj.gameObject.SetActive(true);
    }


}
