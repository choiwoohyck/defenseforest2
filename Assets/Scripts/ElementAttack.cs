using EnumManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementAttack : MonoBehaviour
{
    // Start is called before the first frame update


    
    public float damage;    
    public float attackDelay;

    public bool isAttack = false;
    public bool isBuilding = false;

    GameObject element;

    [SerializeField]
    GameObject target = null;

    public GameObject bulletPrefab;


    void Start()
    {
        element = transform.parent.gameObject;
        damage = element.GetComponent<UnitInfo>().damage;   
        attackDelay = element.GetComponent<UnitInfo>().attackDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.gameObject.GetComponent<Element>().isRoad && transform.parent.gameObject.GetComponent<Element>().elementType != ElementType.ICEROAD)
            return;

        if (target != null && Vector2.Distance(transform.position, target.transform.position) > 3.0f)
            target = null;


        if (!isBuilding)
            return;

        if (target != null && target.GetComponent<Enemy>().hp <= 0)
        {
            target = null;
            isAttack = false;
        }

        if (isAttack && target != null)
        {
            StartCoroutine("Attack");
            isAttack = false;
        }

    }  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target == null && collision.gameObject.CompareTag("Enemy"))
        {
            target = collision.gameObject;
            isAttack = true;

            Debug.Log("OnTriggerEnter2D");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (target == null && collision.gameObject.CompareTag("Enemy"))
        {
            target = collision.gameObject;
            isAttack = true;
            Debug.Log("OnTriggerStay2D");

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == target && target != null)
        {
            target = null;
            isAttack = false;

            Debug.Log("OnTriggerExit2D");

        }
    }

    IEnumerator Attack()
    {
        if (target != null)
        {
            Vector2 startVec = new Vector2(transform.position.x, transform.position.y);
            Vector2 rotVec = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
            rotVec.Normalize();
            if (element.GetComponent<Element>().elementType == ElementType.ICE)
                BulletManager.instance.GetObject(startVec, 15f, rotVec, OwnerType.ICEELEMENT, damage);
            else if (element.GetComponent<Element>().elementType == ElementType.FIRE)
                BulletManager.instance.GetObject(startVec, 10f, rotVec, OwnerType.FIRELEMENT, damage);
            else if (element.GetComponent<Element>().elementType == ElementType.ICEROAD)
                BulletManager.instance.GetObject(startVec, 10f, rotVec, OwnerType.ROADICEELEMENT, damage);

        }

        Debug.Log("Attack");


        yield return new WaitForSeconds(attackDelay);

        isAttack = true;

    }


}
