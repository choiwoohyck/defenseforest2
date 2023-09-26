using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using EnumManagerSpace;
public class Bullet : MonoBehaviour
{


    [SerializeField]
    private float bulletSpeed = 3.0f;

    [SerializeField]
    private float bulletAngle = 0;

    public float damage = 1f;

    private Vector2 rotVec = Vector2.zero;
    private Vector2 startPos = Vector2.zero;
    private Vector2 bulletDirection = Vector2.zero;
    private Rigidbody2D rigid;
    private Collider2D collider2D;
    public OwnerType type;

    Animator animator;

    bool isHit = false;

    GameObject owner;
    GameObject target;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if ((type == OwnerType.PLAYER || type == OwnerType.FIRELEMENT || type == OwnerType.ICEELEMENT || type == OwnerType.ROADICEELEMENT || type == OwnerType.LEAFELEMENT || type == OwnerType.ROADLEAFELEMENT) && !isHit)
        {
            float bulletAngle = (transform.rotation.z+180) * Mathf.Deg2Rad;

            //if (target == null)
            //    bulletDirection = new Vector2(Mathf.Cos(bulletAngle), Mathf.Sin(bulletAngle));
            //else
            //{
            //    bulletDirection = target.transform.position - transform.position;
            //    bulletDirection.Normalize();
            //}
            bulletDirection = new Vector2(Mathf.Cos(bulletAngle), Mathf.Sin(bulletAngle));
            transform.Translate(bulletDirection * bulletSpeed * Time.deltaTime);
        }
    }

    public void Init(Vector2 b_stratPos, float b_Speed, Vector2 b_rotVec, OwnerType o_type,float b_damage,GameObject b_target)
    {
        transform.position = b_stratPos;
        bulletSpeed = b_Speed;
        rotVec = b_rotVec;
        type = o_type;
        damage = b_damage;
        target = b_target;
        transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(rotVec.y,rotVec.x) * Mathf.Rad2Deg + 180);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        collision.gameObject.GetComponent<Enemy>().hp -= Player
    //        .Instance.playerDamage;
    //        //animator.SetBool("isHit", true);
    //        //isHit = true;
    //        //collider2D.isTrigger = true;

    //        BulletPoolManager.ReturnObject(this);

    //        EffectManager.instance.CreateEffect(EffectType.BULLET1HIT, transform.position, bulletDirection);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().hp -= damage;

            if (type == OwnerType.PLAYER)
                EffectManager.instance.CreateEffect(EffectType.BULLET1HIT, transform.position, transform.rotation);
            else if (type == OwnerType.FIRELEMENT)
                EffectManager.instance.CreateEffect(EffectType.FIREELEMENTHIT, transform.position, transform.rotation);
            else if (type == OwnerType.LEAFELEMENT)
            {
                SpriteRenderer renderer = owner.GetComponent<SpriteRenderer>();
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);
                EffectManager.instance.CreateEffect(EffectType.LEAFATTACK, transform.position, transform.rotation);
            }
            else if (type == OwnerType.ICEELEMENT)
            {
                collision.gameObject.GetComponent<Enemy>().frozenCnt++;

                if (collision.gameObject.GetComponent<Enemy>().frozenCnt >= 5)
                {
                    collision.gameObject.GetComponent<Enemy>().isFrozen = true;
                }
            }
            else if (type == OwnerType.ROADLEAFELEMENT)
            {
                EffectManager.instance.CreateEffect(EffectType.LEAFATTACK, transform.position, transform.rotation);
                collision.GetComponent<AddictedComponent>().isAddicted = true;
            }

            if (type != OwnerType.ROADICEELEMENT || type == OwnerType.ROADLEAFELEMENT)
                BulletManager.instance.ReturnObject(gameObject);

            collision.gameObject.GetComponent<HitObject>().ChangeColor();
        }

        if (collision.gameObject.CompareTag("Wall"))
        {

            BulletManager.instance.ReturnObject(gameObject);
            if (type == OwnerType.PLAYER)
                EffectManager.instance.CreateEffect(EffectType.BULLET1HITWALL, transform.position, transform.rotation);
            if (type == OwnerType.ROADICEELEMENT)
                EffectManager.instance.CreateEffect(EffectType.ICICLE, transform.position, transform.rotation);

        }

        if (collision.gameObject.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<UnitInfo>().DecreaseHP(damage);

            if (type == OwnerType.PLAYER)
                EffectManager.instance.CreateEffect(EffectType.BULLET1HIT, transform.position, transform.rotation);

            collision.gameObject.GetComponent<HitObject>().ChangeColor();
            BulletManager.instance.ReturnObject(gameObject);
        }
    }

    public void SetOwner(GameObject owner)
    {
        this.owner = owner;
        SpriteRenderer renderer = owner.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
    }
}
