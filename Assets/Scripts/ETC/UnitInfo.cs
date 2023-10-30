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
    public bool isInvincible = false;

    float invincibleTimer = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;
            if (invincibleTimer > 0.5f)
            {
                invincibleTimer = 0f;
                isInvincible = false;
            }
        }
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
       
        if (!isInvincible)
        {
            isInvincible = true;
            GetComponent<HitObject>().ChangeColor();
            hp -= damage;
        }
    }


}
