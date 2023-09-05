using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public static PlayerInfo Instance;
    public float money = 0;
    public float playerDamage = 10;

    void Start()
    {
        Instance = this;
        StatusInit();
    }

    void StatusInit()
    {
      
        money = 0;
        gameObject.GetComponent<UnitInfo>().StatusInit(100, 1, 10);
        AllyUnitManager.instance.allyUnits.Add(transform.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
