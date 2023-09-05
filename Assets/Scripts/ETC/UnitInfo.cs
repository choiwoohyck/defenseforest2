using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfo : MonoBehaviour
{
    // Start is called before the first frame update

    public float attackDelay = 0.1f;
    public float hp = 100;
    public float maxHp = 100;
    public float damage;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StatusInit(float hp, float attackDelay, float damage)
    {
        this.hp = hp;
        this.attackDelay = attackDelay;
        this.damage = damage;

        maxHp = this.hp;
    }

    public void DecreaseHP(float damage)
    {
        hp -= damage;
    }
}
