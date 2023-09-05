using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStone : MonoBehaviour
{
    // Start is called before the first frame update

    public float hp;
    public float maxHp;

    void init()
    {
        hp = 100;
        maxHp = hp;

        AllyUnitManager.instance.allyUnits.Add(gameObject);
    }

    void Die()
    {
        AllyUnitManager.instance.allyUnits.Remove(gameObject);
    }

    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        hp = GetComponent<UnitInfo>().hp;

        if (hp <= 0)
        {
            GameManager.instance.gameOver = true;
        }
        
    }
}
