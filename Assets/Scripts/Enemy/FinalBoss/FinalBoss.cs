using EnumManagerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class FinalBoss : MonoBehaviour
{
    // Start is called before the first frame update
    public FinalBossState state;
    public int spawnNum = 1;
    public GameObject miniBombPrefab;

    UnitInfo info;

    GameObject Player;
    public GameObject shield;
    Animator animator;

    float patternTimer = 0;
    float waitTimer = 0;
    float explosionTimer = 0;

    public float patterCoolDownTime = 2f;

    bool isWait = false;
    bool isDead = false;
    bool isPunch = false;
    bool isPause = false;
    bool isSpawnFinished = false;
    bool isDrill = false;
    bool isMiddlePunch = true;
    bool isFakeDrill = false;
    bool isRushStop = false;
    bool isReturnRush = false;
    bool canDivideShoot = false;
    public bool isActive = true;


    int explosionCnt = 0;
    public int punchCnt = 0;
    int drillStopnCnt = 0;
    int miniBombCnt = 0;
    int divideShootCnt = 0;

    float shakeIntensity = 0f;
    float punchTimer = 0f;
    float shootTimer = 0f;
    float finalBossRandomAngleOffset;
    float finalBossRandomYOffset;
    public float rushSpeed = 7f;
    float originRushSpeed;
    public float rushAcceleration = 3f;
    float fakeDrillWaitTimer = 0;
    float drillStopFrame = 3f;
    float minibombSpawnStopTimer = 0f;
    float minibombPatternDurationTimer = 0f;

    float moveSpeed = 3.5f;
    float divideShootTimer = 0f;

    Vector3 deadPosition;
    Vector3 drillStartPosition;

    // Start is called before the first frame update

    private void Awake()
    {
        info = GetComponent<UnitInfo>();
        state = FinalBossState.IDLE;
        shield = transform.GetChild(0).gameObject;
    }

    public void init()
    {
        info.StatusInit(10000, 100, 20);
        if (spawnNum == 1)
        {
            transform.position = new Vector3(-10.68f, 6.39f, 0);
        }    
    }



    void Start()
    {
        Player = GameObject.Find("player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        if (GameManager.instance.finalBossKillFail)
        {
            UIManager.instance.killFailBossText.SetActive(true);

            state = FinalBossState.DRILL;
            isWait = true;
            
            
            //isActive = false;
        }

        if (info.hp <= 0 && !isDead)
        {
            deadPosition = transform.position;
            isDead = true;

            shakeIntensity = 2f;

            Debug.Log("isDead IF문");
        }

        if (shakeIntensity > 0)
        {
            transform.position = deadPosition + new Vector3(Random.insideUnitCircle.x * shakeIntensity / 6f, Random.insideUnitCircle.y * shakeIntensity / 6f, 1);
            transform.rotation = Quaternion.Euler(new Vector3(1, 1, Random.insideUnitCircle.x * shakeIntensity * 10f));
            float randomScale = Random.Range(1f, 3f);
            if (explosionTimer >= Time.deltaTime * 3f)
            {
                EffectManager.instance.CreateEffect(EffectType.FIREELEMENTEXPLOSION, transform.position + new Vector3(Random.insideUnitCircle.x * 3f, Random.insideUnitCircle.x * 3f), transform.rotation, new Vector3(randomScale, randomScale, 1f));

                explosionCnt++;
                explosionTimer = 0;
            }

            if (explosionCnt % 15 == 0)
                AudioManager.instance.PlayOnShotSFX(8);

            explosionTimer += Time.deltaTime;
            shakeIntensity -= Time.deltaTime;

        }

        else if (isDead)
        {
            StartCoroutine("Die");

            StageManager.instance.bossKill = true;
            GameManager.instance.energy += 1500;
            isActive = false;
            gameObject.SetActive(false);
        }

        if (isDead) return;

        if (state == FinalBossState.IDLE)
        {
            patternTimer += Time.deltaTime;

            if (patternTimer >= patterCoolDownTime)
            {
                ChangePattern();

                isWait = true;
                patternTimer = 0;
            }
        }



        if (isWait)
        {
            waitTimer += Time.deltaTime;
            if (state == FinalBossState.PUNCH || state == FinalBossState.DRILL)
                transform.position = new Vector3(transform.position.x,Player.transform.position.y,transform.position.z);

            if (GameManager.instance.finalBossKillFail)
                transform.position = new Vector3(transform.position.x, 6f, transform.position.z);

            if (waitTimer >= 1f)
            {
                if (state == FinalBossState.PUNCH)
                    animator.SetBool("isPunch", true);
                if (state == FinalBossState.DRILL)
                {
                    animator.SetBool("isDrill", true);
                    drillStopnCnt = Random.Range(0, 3);
                    isFakeDrill = drillStopnCnt > 0 ? true : false;
                    drillStopFrame = isFakeDrill ? 3 : 11;
                }

                if (state == FinalBossState.SPAWN)
                {
                    animator.SetBool("isSpawn", true);
                }

                if (state == FinalBossState.DIVIDESHOOT)
                {
                    animator.SetBool("isDivideShoot",true);
                    canDivideShoot = true;
                    divideShootCnt++;
                    divideShootTimer = 0f;
                }

                Debug.Log("웨잇 끝나고 애니메이션 전환");
                waitTimer = 0;
                isWait = false;
                isPause = false;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Punch") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 11/15f && state == FinalBossState.PUNCH)
        {
            if (!isPause)
            {
                animator.speed = 0;
                isPause = true;
                isPunch = true;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DivideShoot") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 11 / 15f)
        {
            if (!isPause)
            {
                animator.speed = 0;
                isPause = true;
                canDivideShoot = true;
            }
        }

        if (isPunch)
        {
            punchTimer += Time.deltaTime;
            shootTimer += Time.deltaTime;

            if (punchCnt == 0)
            {
                finalBossRandomAngleOffset = Random.Range(3f, 5f);
                finalBossRandomYOffset = Random.Range(3f, 5f);
            }

            if (shootTimer >= 0.1f)
            {
                shootTimer = 0;

                for (int i = 0; i < 2; i++)
                {
                    GameObject Bullet;
                    if (i == 0) //위에서 생성되는 총알
                    {
                        Bullet = BulletManager.instance.GetPooledObject(transform.position + new Vector3(0.9f, 0.78f), 7f, new Vector2(1, 1), OwnerType.FINALBOSS, 15f, null);
                        Bullet.GetComponent<Bullet>().finalBossRandomOriginAngleOffset = finalBossRandomAngleOffset;
                        Bullet.GetComponent<Bullet>().finalBossRandomAngleOffset = finalBossRandomAngleOffset;
                        Bullet.GetComponent<Bullet>().finalBossRandomYOffset = finalBossRandomYOffset;
                        Bullet.GetComponent<Bullet>().bulletAngle = 0;
                    }
                    else // 아래에서 생성되는 총알
                    {
                        Bullet = BulletManager.instance.GetPooledObject(transform.position + new Vector3(0.9f, -1.3f), 7f, new Vector2(1, 1), OwnerType.FINALBOSS, 15f, null);
                        Bullet.GetComponent<Bullet>().finalBossRandomAngleOffset = finalBossRandomAngleOffset;
                        Bullet.GetComponent<Bullet>().finalBossRandomOriginAngleOffset = finalBossRandomAngleOffset;
                        Bullet.GetComponent<Bullet>().finalBossRandomYOffset = finalBossRandomYOffset * -1f;
                        Bullet.GetComponent<Bullet>().bulletAngle = 0;

                    }

                    punchCnt++;

                    if (spawnNum == 1) //길 위치에 따라 총알 scale 수정
                        Bullet.transform.localScale = new Vector3(-1, 1, 1);
                    else
                        Bullet.transform.localScale = new Vector3(1, 1, 1);
                }
            }

            if (punchTimer >= 8f)
            {
                isPunch = false;
                animator.SetBool("isPunch", false);
                state = FinalBossState.IDLE;
                animator.speed = 1;
                punchTimer = 0;
            }

            if (punchCnt == 10 && isMiddlePunch)
            {
                GameObject Bullet;
                Bullet = BulletManager.instance.GetPooledObject(transform.position, 7f, new Vector2(1, 1), OwnerType.FINALBOSS, 15f, null);
                Bullet.GetComponent<Bullet>().bulletAngle = 0;
                Bullet.GetComponent<Bullet>().finalBossRandomOriginAngleOffset = 0;
                Bullet.GetComponent<Bullet>().finalBossRandomAngleOffset = 0;
                Bullet.GetComponent<Bullet>().finalBossRandomYOffset = 0;
                isMiddlePunch = false; // 가운데 한발
            }

            if (punchCnt == 20)
            {
                isMiddlePunch = true;
                punchCnt = 0;
            }
        }

        if (canDivideShoot)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject Bullet = BulletManager.instance.GetPooledObject(transform.position + new Vector3(0.9f, 0.78f), 7f, new Vector2(1, 1), OwnerType.FINALBOSS, 15f, null, null, true, 0);
                Bullet.gameObject.transform.rotation = Quaternion.Euler(0, 0, (i-2) * 30);

            }

            AudioManager.instance.PlayOnShotSFX(7);

            Debug.Log("총알 1개 발사");
            canDivideShoot = false;
          
        }

        if (state == FinalBossState.DIVIDESHOOT)
        {
            divideShootTimer += Time.deltaTime;

            if (divideShootTimer >= 2f)
            {
                divideShootTimer = 0;
                canDivideShoot = true;
                divideShootCnt++;
                animator.Play("SpawnBomb", -1, 0f);

            }

            if (divideShootCnt > 2)
            {
                divideShootCnt = 0;
                state = FinalBossState.IDLE;
                animator.SetBool("isDivideShoot", false);
                isPause = false;
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Drill") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= drillStopFrame / 13f)
        {
            if (!isPause)
            {
                animator.speed = 0;
                drillStartPosition = transform.position;
                drillStopnCnt = Random.Range(0, 3);
                isFakeDrill = drillStopnCnt > 0 ? true : false;
                originRushSpeed = rushSpeed;
                isDrill = true;
                isReturnRush = false;
                isPause = true;


                Debug.Log("isPause drillStopCnt :" + drillStopnCnt + " isFakeDrill : " + isFakeDrill);
            }
        }

        if (isDrill && state == FinalBossState.DRILL)
        {
            if (!isRushStop)
            {
                float xPos = transform.position.x;

                if (isFakeDrill)
                    xPos += rushSpeed * Time.deltaTime * 2;
                else
                    xPos += rushSpeed * Time.deltaTime;
                transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
            }

            if (GameManager.instance.finalBossKillFail)
            {
                float distance = Vector2.Distance(transform.position, GameObject.Find("MagicStone").transform.position);

                if (distance <= 0.5f)
                {
                    GameObject.Find("MagicStone").GetComponent<UnitInfo>().DecreaseHP(100000);
                }
            }

            if (!isFakeDrill)
            {
                rushSpeed += rushAcceleration * Time.deltaTime;
            }

            if (transform.position.x >= 10 && !isReturnRush)
            {
                Debug.Log("반환점 찍고 역방향");
                isReturnRush = true;
                animator.Play("Drill");
                rushSpeed *= -1;
                transform.localScale = new Vector3(transform.localScale.x * -1f, 1.5f, 1);
            }


            if (rushSpeed < 0 && transform.position.x <= drillStartPosition.x)
            {
                Debug.Log("돌진 종료");
                transform.position = drillStartPosition;
                rushSpeed = originRushSpeed;
                isDrill = false;
                transform.localScale = new Vector3(transform.localScale.x * -1f, 1.5f, 1);

                animator.SetBool("isDrill", false);
                state = FinalBossState.IDLE;
            }

            if (isFakeDrill)
            {
                if (drillStopnCnt > 0)
                {
                    float offset = drillStopnCnt == 1 ? 4 : 2;
                    if (transform.position.x - offset > drillStartPosition.x)
                    {
                        isRushStop = true;
                        drillStopnCnt--;
                    }

                }
            }
            else
                animator.speed = 1f;

            if (isRushStop)
            {
                rushSpeed = 0;
                fakeDrillWaitTimer += Time.deltaTime;

                if (fakeDrillWaitTimer >= 0.2f)
                {
                    isRushStop = false;
                    rushSpeed = originRushSpeed;
                    fakeDrillWaitTimer = 0;

                    if (drillStopnCnt == 0)
                    {
                        isFakeDrill = false;
                        animator.speed = 1f;
                    }
                }

            }

        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("SpawnBomb") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 3 / 8f && miniBombCnt < 10f)
        {
            animator.speed = 0f;
            minibombSpawnStopTimer += Time.deltaTime;

            if (minibombSpawnStopTimer >= 0.5f)
            {
                animator.speed = 1f;

                GameObject miniBomb = Instantiate(miniBombPrefab) as GameObject;
                miniBomb.transform.position = transform.position;

                miniBomb.GetComponent<MiniBomb>().Init(new Vector2(Random.Range(0, 28), Random.Range(0, 18)));
                miniBombCnt++;

                if (miniBombCnt <= 9f)
                    animator.Play("SpawnBomb", -1, 0f);
                else
                {
                    //shield.SetActive(true);
                    isSpawnFinished = true;
                }

                minibombSpawnStopTimer = 0f;
            }
        }

        if (isSpawnFinished)
        {
            minibombPatternDurationTimer += Time.deltaTime;
            if (minibombPatternDurationTimer >= 10f)
            {
                //shield.SetActive(false);
                animator.SetBool("isSpawn", false);
                state = FinalBossState.IDLE;

                minibombPatternDurationTimer = 0f;
                isSpawnFinished = false;
                miniBombCnt = 0;

            }

        }
    }
    void ChangePattern()
    {
        int randomStateNum = Random.Range(1, 5);
        //randomStateNum = 4;
        state = (FinalBossState)randomStateNum;
    }

    IEnumerator Die()
    {
        animator.speed = 0.0f;

        Color rendererColor = GetComponent<SpriteRenderer>().color;

        while (rendererColor.a > 0)
        {
            rendererColor.a -= Time.deltaTime * 2;
            GetComponent<SpriteRenderer>().color = rendererColor;
            yield return null;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponent<UnitInfo>().isInvincible || collision.gameObject.GetComponent<UnitInfo>().hp <= 0) return;

            float rushDamage = isDrill ? 30f : 10f;

            collision.gameObject.GetComponent<UnitInfo>().DecreaseHP(rushDamage);
        }
    }
    
}

