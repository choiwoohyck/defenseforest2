using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserComponent : MonoBehaviour
{

    bool isAttack = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime <= 8/17f && !isAttack && collision.CompareTag("Player"))
        {
            collision.GetComponent<UnitInfo>().DecreaseHP(30f);
            isAttack = true;
        }
    }
}
