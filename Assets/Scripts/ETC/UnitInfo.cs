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
    public int roadNum = -1;
    float invincibleTimer = 0f;

    public int targetedNum = 0;


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
            if (!gameObject.CompareTag("MagicStone"))
            {
                isInvincible = true;
                GetComponent<HitObject>().ChangeColor();
            }

            hp -= damage;

            if (hp <= 0 && gameObject.CompareTag("Player"))
            {
                if (!gameObject.GetComponent<DeadComponent>().alreadyWork)
                {
                    gameObject.GetComponent<DeadComponent>().InitSetting();
                    gameObject.GetComponent<DeadComponent>().alreadyWork = true;
                }
            }

            if (hp <= 0 && gameObject.CompareTag("MagicStone"))
            {

                GameObject.Find("player").GetComponent<DeadComponent>().isPlayerDead = false;

                if (!GameObject.Find("player").GetComponent<DeadComponent>().alreadyWork)
                {
                    GameObject.Find("player").GetComponent<DeadComponent>().InitSetting();
                    GameObject.Find("player").GetComponent<DeadComponent>().alreadyWork = true;
                }
                
            }
        }

        
    }


}
