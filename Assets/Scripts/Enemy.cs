using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;

    MonsterType type;
    public EnemyState state = EnemyState.SEARCH;


    public float hp;
    public float maxhp;
    public float damage;
    public float moveSpeed;
    public float originMoveSpeed;
    public int frozenCnt = 0;


    public float attackDistance = 1f;
    public bool isDead = false;
    public bool isFrozen = false;

    bool isAttack = false;
    bool isRush = false;


    float searchTimer = 0;
    float attackTimer = 0;
    float rushStartTimer = 0;
    float rushTimer = 0;


    Animator animator;
    Rigidbody2D rigidbody2D;

    Vector3 direction = Vector3.zero;
    public void StatusInit(MonsterType m_type)
    {
        type = m_type;

        if (type == MonsterType.ZOMBIE)
        {
            attackDistance = 1.0f;
            damage = 5;
            hp = 100;
            moveSpeed = 2f;
        }

        if(type == MonsterType.FRANKSTEIN)
        {
            attackDistance = 1.0f;
            damage = 10;
            hp = 50;
            moveSpeed = 3f;
        }

        maxhp = hp;
        originMoveSpeed = moveSpeed;
         
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        EnterState(state);
        rigidbody2D = GetComponent<Rigidbody2D>();
        //EnemyUnitManager.instance.enemyUnits.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {             

        if (target == GameObject.Find("player") && !TileMapManager.instance.isPlayerRoad())
        {
            target = null;
            EnterState(EnemyState.SEARCH);
        }

        if (hp <= 0 && !isDead)
        {
            StartCoroutine("Die");
            isDead = true;
        }

        if (hp > 0) 
        {        
            updateState();
        }

        if (isFrozen)
        {
            StartCoroutine("Frozen");
            isFrozen = false;
            frozenCnt = 0;
        }    
    }

    void EnterState(EnemyState currentState)
    {
        state = currentState;

        if (currentState == EnemyState.SEARCH)
        {
            target = GameObject.Find("MagicStone");
            float minDistance = Vector3.Distance(transform.position, target.transform.position);
            foreach (var ally in AllyUnitManager.instance.allyUnits)
            {

                if (target == ally || ally.GetComponent<UnitInfo>().hp <= 0) continue;
                if (ally.CompareTag("Player") && !TileMapManager.instance.isPlayerRoad()) continue;
                if (!TileMapManager.instance.isSameRoad(ally.transform.position,transform.position)) continue;

                float distance = Vector3.Distance(transform.position, ally.transform.position);
        
                if (distance < minDistance)
                {
                    target = ally;
                    minDistance = distance;
                }
            }

            ChangeState(EnemyState.RUN);
        }
    }

    void updateState()
    { 
        if (target == null)
        {
            ChangeState(EnemyState.SEARCH);
        }

        if (state == EnemyState.RUN && target != null)
        {

            if (searchTimer >= 1f)
            {
                ChangeState(EnemyState.SEARCH);
                searchTimer = 0;
            }
           
            if (target.CompareTag("Player"))
            {
                if (!isRush)
                 rushStartTimer += Time.deltaTime;

                if (rushStartTimer >= 1.5f)
                {
                    isRush = true;

                    animator.speed = 2.0f;
                    direction = (target.transform.position - transform.position).normalized;

                    rushStartTimer = 0;
                }                
            }

            if (!isRush)
            {
                direction = (target.transform.position - transform.position).normalized;
                searchTimer += Time.deltaTime;

                Debug.DrawRay(transform.position, direction * 0.75f, new Color(0, 1, 0));
                RaycastHit2D[] rayHits = Physics2D.RaycastAll(transform.position, direction, 0.35f);
                foreach (RaycastHit2D rayHit in rayHits)
                {
                    if (rayHit.collider.gameObject.CompareTag("Enemy") && rayHit.collider.gameObject != gameObject)
                    {
                        Debug.Log("¿ä±â");
                        ChangeState(EnemyState.READY);
                        animator.SetBool("isReady", true);

                    }
                }
            }

            else
            {
                rushTimer += Time.deltaTime;
                moveSpeed = originMoveSpeed * 1.75f;

                //transform.position += direction * moveSpeed * Time.deltaTime;
                rigidbody2D.MovePosition(rigidbody2D.position + new Vector2(direction.x,direction.y) * moveSpeed * Time.fixedDeltaTime);
                rigidbody2D.velocity = Vector3.zero;
                rigidbody2D.angularVelocity = 0;

                if (rushTimer >= 0.3f)
                {
                    isRush = false;
                    moveSpeed = originMoveSpeed;
                    animator.speed = 1.75f;
                    rushTimer = 0;
                }
                
            }

            animator.SetBool("isAttack", false);
            direction.Normalize();

            animator.SetFloat("xDir", direction.x);
            animator.SetFloat("yDir", direction.y);

            if (Vector2.Distance(transform.position, target.transform.position) < 0.8f && target != GameObject.Find("MagicStone"))
            {

                ChangeState(EnemyState.ATTACK);
                animator.SetBool("isAttack", true);
                isAttack = true;
            }
            else
            {
                rigidbody2D.MovePosition(rigidbody2D.position + new Vector2(direction.x, direction.y) * moveSpeed * Time.fixedDeltaTime);
                rigidbody2D.velocity = Vector3.zero;
                rigidbody2D.angularVelocity = 0;
            }              
        }

        if (state == EnemyState.ATTACK)
        {
            attackTimer += Time.deltaTime;
            rigidbody2D.velocity = Vector3.zero;
            rigidbody2D.angularVelocity = 0;

            if (attackTimer >= 1)
            {
                isAttack = false;
                attackTimer = 0;
            }

            if (Vector2.Distance(transform.position, target.transform.position) > 0.9f && target != GameObject.Find("MagicStone")) 
            {

                ChangeState(EnemyState.SEARCH);
                animator.SetBool("isAttack", false);

            }

        }

        if (state == EnemyState.READY)
        {
            RaycastHit2D[] rayHits = Physics2D.RaycastAll(transform.position, direction, 0.35f);
            int cnt = 0;
            foreach (RaycastHit2D rayHit in rayHits)
            {
                if (rayHit.collider.gameObject.CompareTag("Enemy") && rayHit.collider.gameObject != gameObject)
                {
                    cnt++;
                }
            }

            if (cnt == 0)
            {
                ChangeState(EnemyState.SEARCH);
                animator.SetBool("isReady", false);
            }
        }
    }

    void ChangeState(EnemyState nextState)
    {
        EnterState(nextState); 
    }

    void ZombieAttack()
    {
        
        if (target == null)
        {         
            ChangeState(EnemyState.SEARCH);
        }

        if (target!= null && !isAttack)
        {
            if (target == GameObject.Find("MagicStone"))
            {
                target.GetComponent<UnitInfo>().hp -= damage;
                hp = 0;
                isAttack = true;

                return;
            }
            else
            {
                target.GetComponent<UnitInfo>().hp -= damage;
                target.GetComponent<HitObject>().ChangeColor();
                isAttack = true;
            }
        }
    }

    IEnumerator Die()
    {
        animator.speed = 0.0f;

        Color rendererColor = GetComponent<SpriteRenderer>().color;

        while (rendererColor.a > 0)
        {
            rendererColor.a -= Time.deltaTime*2;
            GetComponent<SpriteRenderer>().color = rendererColor;
            yield return null;
        }


        Destroy(gameObject);
    }

    IEnumerator Frozen()
    { 
        animator.speed = 0.0f;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color originColor = renderer.color;
        renderer.color = Color.blue;
        moveSpeed = 0;
        yield return new WaitForSeconds(0.5f);

        animator.speed = 1;
        moveSpeed = originMoveSpeed;
        renderer.color = originColor;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rigidbody2D.velocity = Vector2.zero;

        rushTimer = 1;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<UnitInfo>().hp -= damage;
            collision.gameObject.GetComponent<HitObject>().ChangeColor();
            hp = 0;
        }

        if (collision.gameObject.name == ("MagicStone"))
        {
            animator.SetBool("isAttack", true);
            ChangeState(EnemyState.ATTACK);
        }

        Debug.Log(collision.gameObject);

        
    }

}
