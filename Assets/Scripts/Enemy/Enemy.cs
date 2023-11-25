using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    public GameObject prevTarget;


    public MonsterType type;
    public EnemyState state = EnemyState.SEARCH;


    public float hp;
    public float maxhp;
    public float damage;
    public float moveSpeed;
    public float originMoveSpeed;
    public float waitNum = 0;
    public int frozenCnt = 0;


    public float attackDistance = 1f;
    public bool isDead = false;
    public bool isFrozen = false;
    public bool noEnergy = false;
    

    public int spawnNum;

    bool isAttack = false;
    bool isRush = false;
    bool isFirstTarget = false;


    float searchTimer = 0;
    float attackTimer = 0;
    float rushStartTimer = 0;
    float rushTimer = 0;
    float spawnTimer = 0;


    Animator animator;
    Rigidbody2D rigidbody2D;
    BoxCollider2D collider2D;

    Vector3 direction = Vector3.zero;
    public void StatusInit(MonsterType m_type)
    {
        type = m_type;

        if (type == MonsterType.ZOMBIE)
        {
            attackDistance = 1.0f;
            damage = 5;
            hp = 90 + GameManager.instance.Stage * 10f;

            
            moveSpeed = 2f + 0.15f * GameManager.instance.Stage;
        }

        if(type == MonsterType.FRANKSTEIN)
        {
            attackDistance = 1.0f;
            damage = 10;
            hp = 140 + GameManager.instance.Stage * 10f;

            moveSpeed = 2.9f + 0.15f * GameManager.instance.Stage;
        }

        maxhp = hp;
        originMoveSpeed = moveSpeed;

        isDead = false;
        isFrozen = false;
        noEnergy = false;
        isAttack = false;
        isRush = false;
        isFirstTarget = false;

        searchTimer = 0;
        attackTimer = 0;
        rushStartTimer = 0;
        rushTimer = 0;
        spawnTimer = 0;
        waitNum = 0;

        direction = Vector3.zero;
        state = EnemyState.SEARCH;
        animator = GetComponent<Animator>();
        animator.speed = 1;
        target = null;
        prevTarget = null;

        GetComponent<AddictedComponent>().shakeIntensity = 0;
        GetComponent<AddictedComponent>().isAddicted = false;

        //GetComponent<HitObject>().ChangeOriginColor();
        GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
        collider2D.isTrigger = false;

    }
    void Start()
    {
        animator = GetComponent<Animator>();
        EnterState(state);
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
        EnemyUnitManager.instance.enemyUnits.Add(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
        spawnTimer += Time.deltaTime;

        if (target == GameObject.Find("player") && (!(TileMapManager.instance.isPlayerRoad()) || TileMapManager.instance.playerRoadNum  != spawnNum))
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
            direction = (target.transform.position  - transform.position).normalized;

            float minDistance = Vector3.Distance(transform.position, target.transform.position);
            foreach (var ally in AllyUnitManager.instance.allyUnits)
            {
                Debug.Log(ally);
                if (target == ally || ally.GetComponent<UnitInfo>().hp <= 0) continue;
                if (ally.CompareTag("Player") && (!TileMapManager.instance.isPlayerRoad() || TileMapManager.instance.playerRoadNum != spawnNum)) continue;
                if (ally.CompareTag("Element") && ally.GetComponent<UnitInfo>().roadNum != spawnNum) continue;
                if (ally.GetComponent<UnitInfo>().targetedNum >= 2) continue;

                float distance = Vector3.Distance(transform.position, ally.transform.position);
        
                if (distance < minDistance)
                {
                    if (target != ally)
                    {
                        Debug.Log(target + " " + ally);
                        target = ally;
                        minDistance = distance;
                    }
                }
            }

            if (target.CompareTag("Element"))
            {
                target.GetComponent<UnitInfo>().targetedNum++;
            }

            if (spawnNum == 1 || spawnNum == 3)
                direction = (target.transform.position + new Vector3(0, Random.Range(-0.5f, 0.5f)) - transform.position);
            else if (spawnNum == 2 || spawnNum == 4)
                direction = (target.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0) - transform.position);

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
            searchTimer += Time.deltaTime;


            if (!target.CompareTag("Player") && TileMapManager.instance.isPlayerRoad() && TileMapManager.instance.playerRoadNum == spawnNum)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);

                if (distance > Vector3.Distance(transform.position, GameObject.Find("player").transform.position))
                {
                    if (target.CompareTag("Element"))
                    {
                        target.GetComponent<UnitInfo>().targetedNum--;
                    }

                    target = GameObject.Find("player");
                    searchTimer = 0;
                }
            }

            foreach (var ally in AllyUnitManager.instance.allyUnits)
            {
                Debug.Log(ally);
                if (target == ally || ally.GetComponent<UnitInfo>().hp <= 0) continue;
                if (ally.CompareTag("Player") && (!TileMapManager.instance.isPlayerRoad() || TileMapManager.instance.playerRoadNum != spawnNum)) continue;
                if (ally.CompareTag("Element") && ally.GetComponent<UnitInfo>().roadNum != spawnNum) continue;

                if (Vector2.Distance(transform.position, ally.transform.position) < 0.95f && ally != GameObject.Find("MagicStone") && !ally.CompareTag("Player"))
                {
                    ChangeState(EnemyState.ATTACK);
                    target = ally;
                    animator.SetBool("isAttack", true);
                    isAttack = true;
                }
            }
            /*
                //if (searchTimer >= 0.5f)
                //{
                //    if (target.CompareTag("Element"))
                //        target.GetComponent<UnitInfo>().targetedNum--;

                //    prevTarget = target;
                //    ChangeState(EnemyState.SEARCH);
                //    searchTimer = 0;
                //}
            */

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


            // 앞에 다른 적있을때 멈추는 코드
            if (!isRush && target == GameObject.Find("MagicStone"))
            {
                //direction = (target.transform.position - transform.position).normalized;
                searchTimer += Time.deltaTime;

                Debug.DrawRay(transform.position, direction * 0.35f, new Color(0, 1, 0));
                //RaycastHit2D[] rayHits = Physics2D.RaycastAll(transform.position, direction, 0.35f);
                //foreach (RaycastHit2D rayHit in rayHits)
                //{
                //    if (rayHit.collider.gameObject.CompareTag("Enemy") && rayHit.collider.gameObject != gameObject && spawnTimer >= 5f)
                //    {
                //        ChangeState(EnemyState.READY);
                //        animator.SetBool("isReady", true);
                //    }
                //}
            }

            else if (isRush)
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

            if (Vector2.Distance(transform.position, target.transform.position) < 0.95f && target != GameObject.Find("MagicStone") && !target.CompareTag("Player"))
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
            collider2D.isTrigger = true;

            if (attackTimer >= 1)
            {
                isAttack = false;
                attackTimer = 0;
            }

            if (Vector2.Distance(transform.position, target.transform.position) > 1.0f && target != GameObject.Find("MagicStone")) 
            {
                ChangeState(EnemyState.RUN);
                animator.SetBool("isAttack", false);
                collider2D.isTrigger = false;
            }

        }

        if (state == EnemyState.READY)
        {
            RaycastHit2D[] rayHits = Physics2D.RaycastAll(transform.position, direction, 0.35f);
            int cnt = 0;

            waitNum += Time.deltaTime;

            if (waitNum >= 3f)
            {
                ChangeState(EnemyState.SEARCH);
                waitNum = 0;
            }

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
                //target.GetComponent<UnitInfo>().hp -= damage;
                target.GetComponent<UnitInfo>().DecreaseHP(damage);
                noEnergy = true;
                hp = 0;
                isAttack = true;
                collider2D.isTrigger = true;
                return;
            }
            else
            {
                target.GetComponent<UnitInfo>().DecreaseHP(damage);

                if (target.CompareTag("Element") && target.GetComponent<UnitInfo>().hp <= 0)
                {
                    Debug.Log("죽였습니당");
                    TileMapManager.instance.changeBuildableTrue((int)target.GetComponent<Element>().tileMapPos.x, (int)target.GetComponent<Element>().tileMapPos.y);
                    Debug.Log((int)target.GetComponent<Element>().tileMapPos.x + " , " + (int)target.GetComponent<Element>().tileMapPos.y);
                }

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

        if (!noEnergy)
            GameManager.instance.energy += 5;

        EnemyUnitManager.instance.enemyUnits.Remove(gameObject);

        if (target != null && target.CompareTag("Element"))
            target.GetComponent<UnitInfo>().targetedNum--;

        EnemyUnitManager.instance.enemyUnits.Remove(gameObject);
        EnemyPool.instance.ReturnObject(gameObject);
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
            if (collision.gameObject.GetComponent<UnitInfo>().isInvincible || collision.gameObject.GetComponent<UnitInfo>().hp <= 0) return;

            collision.gameObject.GetComponent<UnitInfo>().DecreaseHP(damage);
            collider2D.isTrigger = true;
            noEnergy = true;
            hp = 0;
        }

        if (collision.gameObject.name == ("MagicStone"))
        {
            animator.SetBool("isAttack", true);
            collider2D.isTrigger = true;
            ChangeState(EnemyState.ATTACK);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {

            collision.gameObject.GetComponent<UnitInfo>().DecreaseHP(damage);
            collision.gameObject.GetComponent<HitObject>().ChangeColor();
            hp = 0;
        }

        if (collision.gameObject.name == ("MagicStone"))
        {
            animator.SetBool("isAttack", true);
            collider2D.isTrigger = true;
            ChangeState(EnemyState.ATTACK);
        }
    }



    bool InCamera()
    {
        Vector3 cameraPos = Camera.main.WorldToViewportPoint(transform.position);
        return (cameraPos.x < 1f && cameraPos.y < 1f && cameraPos.x > 0f && cameraPos.y > 0f);
    }
}
