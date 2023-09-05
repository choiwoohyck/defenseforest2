using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonComponent : MonoBehaviour
{
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
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().moveSpeed /= 2;
            collision.GetComponent<AddictedComponent>().isAddicted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().moveSpeed = collision.GetComponent<Enemy>().originMoveSpeed;
        }
    }
}
