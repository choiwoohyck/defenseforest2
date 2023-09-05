using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
using System.Drawing;
using Unity.VisualScripting;

public class FireElementRoad : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isBomb = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
;

        if (!transform.parent.gameObject.GetComponent<Element>().isRoad)
            return;

        if (!isBomb && transform.parent.gameObject.GetComponent<UnitInfo>().hp <= 0)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 6);
            //List<GameObject> enemyList = new List<GameObject>();
            foreach(Collider2D hit in hits)
            {
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    //enemyList.Add(hit.gameObject);
                    hit.GetComponent<Enemy>().hp -= 100;
                }
            }

            EffectManager.instance.CreateEffect(EffectType.FIREELEMENTEXPLOSION, transform.position, transform.rotation);
            transform.parent.gameObject.GetComponent<Element>().isRoad = false;
            isBomb = true;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.gameObject.CompareTag("Enemy") && transform.parent.gameObject.GetComponent<UnitInfo>().hp <= 0 && !isBomb)
        //{
        //    collision.gameObject.GetComponent<Enemy>().hp -= 100;
        //    EffectManager.instance.CreateEffect(EffectType.FIREELEMENTEXPLOSION, transform.position, transform.rotation);
        //    transform.parent.gameObject.GetComponent<Element>().isRoad = false;
        //    isBomb = true;
        //}

    }
}
