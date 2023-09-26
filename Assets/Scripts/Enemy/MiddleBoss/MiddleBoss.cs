using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
public class MiddleBoss : MonoBehaviour
{
    MiddleBossState state;
    UnitInfo info;

    public GameObject laserPrefab;

    // Start is called before the first frame update

    private void Awake()
    {
       info = GetComponent<UnitInfo>();
       state = MiddleBossState.IDLE;
    }

    void init()
    {
        info.StatusInit(1000, 100, 20);
    }

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateLaser()
    {
        GameObject laser = Instantiate(laserPrefab);
        laser.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
    }
}
