using EnumManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementAttack : MonoBehaviour
{
    // Start is called before the first frame update


    
    public float damage;    
    public float attackDelay;
    float attackTimer = 0;

    public float bulletSpeed;

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
        attackTimer = attackDelay;
    }

    public void init()
    {
        element = transform.parent.gameObject;
        damage = element.GetComponent<UnitInfo>().damage;
        attackDelay = element.GetComponent<UnitInfo>().attackDelay;
        attackTimer = attackDelay;
    }


    // Update is called once per frame
    void Update()
    {
        damage = element.GetComponent<UnitInfo>().damage;
        attackDelay = element.GetComponent<UnitInfo>().attackDelay;

        if (transform.parent.gameObject.GetComponent<Element>().isRoad && transform.parent.gameObject.GetComponent<Element>().elementType != ElementType.ICEROAD && transform.parent.gameObject.GetComponent<Element>().elementType != ElementType.LEAFROAD && transform.parent.gameObject.GetComponent<Element>().elementType != ElementType.STONE)
            return;

        attackTimer += Time.deltaTime;


        if (target != null && target.GetComponent<Enemy>().hp <= 0)
        {
            target = null;
            isAttack = false;
        }

        if (!isBuilding)
            return;

        if (isAttack && target != null && attackTimer >= attackDelay)
        {
            StartCoroutine("Attack");
            attackTimer = 0;
            Debug.Log("Attack!!");
        }

    }  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target == null && collision.gameObject.CompareTag("Enemy"))
        {
            target = collision.gameObject;
            isAttack = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (target == null && collision.gameObject.CompareTag("Enemy"))
        {
            target = collision.gameObject;
            isAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == target && target != null)
        {
            target = null;
            Debug.Log("targetNull");
            isAttack = false;
        }
    }

    IEnumerator Attack()
    {
        if (target != null)
        {
            Vector2 startVec = new Vector2(transform.position.x, transform.position.y);
            Vector2 rotVec = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);

            rotVec.Normalize();

            OwnerType type = OwnerType.ICEELEMENT;
            float speed = 20f;

            if (element.GetComponent<Element>().elementType == ElementType.ICE)
            {
                type = OwnerType.ICEELEMENT;
            }
            else if (element.GetComponent<Element>().elementType == ElementType.FIRE)
            {
                type = OwnerType.FIRELEMENT;
            }
            else if (element.GetComponent<Element>().elementType == ElementType.ICEROAD)
            {
                type = OwnerType.ROADICEELEMENT;
            }
            else if (element.GetComponent<Element>().elementType == ElementType.LEAF)
            {
                type = OwnerType.LEAFELEMENT;
            }
            else if (element.GetComponent<Element>().elementType == ElementType.LEAFROAD)
            {
                type = OwnerType.ROADLEAFELEMENT;
                speed = 7f;
            }

            GameObject bullet = BulletManager.instance.GetPooledObject(startVec, speed, rotVec, type, damage, target, transform.parent.gameObject);
        }

        isAttack = true;

        yield return  null;

    }


}
