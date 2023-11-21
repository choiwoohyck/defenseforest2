using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
public class MiniBomb : MonoBehaviour
{
    // Start is called before the first frame update

    public float moveSpeed = 5.0f;
    public float maxMoveSpeed;
    public float lifeTimer = 0;
    public Vector3 targetPos;
    Vector3 direction;
    UnitInfo info;

    bool isWalk = false;
    bool isChase = false;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        info = GetComponent<UnitInfo>();
        maxMoveSpeed = Random.Range(2.5f, 3.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (info.hp <= 0f)
        {
            EffectManager.instance.CreateEffect(EffectType.MINIBOMBEXPLOSION, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (isWalk) 
        {
            if (transform.position.x < targetPos.x)
                transform.localScale = new Vector3(8f, 8f, 1f);
            else
                transform.localScale = new Vector3(-8f, 8f, 1f);

            if (!isChase)
            {
                animator.SetBool("isWalk", true);
            }
            else
            {
                targetPos = GameObject.Find("player").transform.position;
                moveSpeed += Time.deltaTime;
                lifeTimer += Time.deltaTime;

                if (lifeTimer >= 5f)
                {
                    EffectManager.instance.CreateEffect(EffectType.MINIBOMBEXPLOSION, transform.position, transform.rotation);
                    AudioManager.instance.PlayOnShotSFX(8);

                    Destroy(gameObject);
                    lifeTimer = 0;
                }
            }

            

            direction = (targetPos - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            float rotationZ = transform.rotation.z +Time.deltaTime;
            if (moveSpeed >= maxMoveSpeed)
            {
                moveSpeed = maxMoveSpeed;
            }
            transform.rotation = Quaternion.Euler(new Vector3 (0,0,rotationZ));
        }

        if (Vector3.Distance(transform.position,targetPos) <= 0.01f && !isChase)
        {
            transform.position = new Vector3(targetPos.x, targetPos.y);
            animator.SetBool("isWalk", false);
            isChase = true;
            moveSpeed = 0f;
        }


      
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MiniBomb") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 5 / 6f)
            animator.Play("MiniBomb", -1, 0f);
    }

    public void Init(Vector2 _targetPos)
    {
        isWalk = true;
        targetPos = TileMapManager.instance.ChangeTilePosionToRealPosition(_targetPos);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<UnitInfo>().isInvincible) return;

            collision.GetComponent<UnitInfo>().DecreaseHP(20f);
            EffectManager.instance.CreateEffect(EffectType.MINIBOMBEXPLOSION, transform.position, transform.rotation);
            AudioManager.instance.PlayOnShotSFX(8);

            Destroy(gameObject);

        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<UnitInfo>().isInvincible) return;

            collision.GetComponent<UnitInfo>().DecreaseHP(20f);
            EffectManager.instance.CreateEffect(EffectType.MINIBOMBEXPLOSION, transform.position, transform.rotation);
            AudioManager.instance.PlayOnShotSFX(8);

            Destroy(gameObject);

        }
    }
}
