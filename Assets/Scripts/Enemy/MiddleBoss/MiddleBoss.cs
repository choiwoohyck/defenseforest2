#if UNITY_EDITER
using UnityEditor.ShaderGraph.Internal;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
using UnityEngine.Rendering;
using System.Runtime.Serialization.Json;

public class MiddleBoss : MonoBehaviour
{
    public MiddleBossState state;
    UnitInfo info;

    public GameObject laserPrefab;
    public GameObject laserBoundaryPrefab;
    public GameObject bossActiveRange;

    GameObject Player;
    Animator animator;

    float patternTimer = 0;
    float waitTimer = 0;
    float laserAngle = 0;
    float machineTimer = 0;
    float explosionTimer = 0;
    float machineAngle = 0;

    public float patterCoolDownTime = 2f;

    bool isWait = false;
    bool isDead = false;
    bool isMachine = false;
    bool isFinalLaser = false;
    public bool isActive = false;

    int laserPattern = 0;
    int shootPattern = 0;
    int machineBulletCnt = 0;
    int lastPattern = 0;
    int explosionCnt = 0;

    float shakeIntensity = 0f;
    Vector3 deadPosition;

    // Start is called before the first frame update

    private void Awake()
    {
       info = GetComponent<UnitInfo>();
       state = MiddleBossState.IDLE;
       EnemyUnitManager.instance.enemyUnits.Add(gameObject);
    }

    void init()
    {
        info.StatusInit(3000, 100, 20);
    }

    

    void Start()
    {
        Player = GameObject.Find("player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.middleBossKillFail && !isFinalLaser)
        {
            UIManager.instance.killFailBossText.SetActive(true);
            AudioManager.instance.PlayOnShotSFX(5);
            
            GameObject laser = Instantiate(laserPrefab);
            laser.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
            laser.transform.rotation = Quaternion.Euler(0, 0, 270);
            laser.GetComponent<SpriteRenderer>().color = new Color(240, 0, 0);
            StartCoroutine("BreakMagicStone");
            isFinalLaser = true;
            isActive = false;
        }

        if (!isActive) return;

        if (info.hp <= 0 && !isDead)
        {
            deadPosition = transform.position;
            isDead = true;
            
            shakeIntensity = 2f;

            Debug.Log("isDead IF문");
        }

        if (shakeIntensity > 0)
        {
            transform.position = deadPosition + new Vector3(Random.insideUnitCircle.x * shakeIntensity / 6f , Random.insideUnitCircle.y * shakeIntensity / 6f, 1);
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
            GameManager.instance.energy += 1000;
            bossActiveRange.GetComponent<BossActiveRange>().active = false;
            isActive = false;
            Player.gameObject.GetComponent<UnitInfo>().hp = Player.gameObject.GetComponent<UnitInfo>().maxHp;
            gameObject.SetActive(false);
        }

        if (isDead) return;
        if (state == MiddleBossState.IDLE)
        {
            patternTimer += Time.deltaTime;
            
            if (patternTimer >= patterCoolDownTime)
            {
                
                ChangePattern();

                if (state == MiddleBossState.LASER)
                    CreateBoundary();

                if (state == MiddleBossState.GUN)
                    Shoot();

                if (state == MiddleBossState.MACHINEGUN)
                {
                    isMachine = true;
                    machineBulletCnt = 0;
                    machineAngle = -40;
                }

                isWait = true;
                patternTimer = 0;
            }
        }

        if (state == MiddleBossState.LASER && !isWait)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                animator.SetBool("isLaser", false);
                state = MiddleBossState.IDLE;
            }
        }

        if (state == MiddleBossState.GUN)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                animator.SetBool("isShoot", false);
                state = MiddleBossState.IDLE;
            }
        }

        if (state == MiddleBossState.MACHINEGUN)
        {
            
            if(isMachine)
            {
                machineTimer += Time.deltaTime;
                if (machineTimer >= 0.15f)
                {
                    GameObject Bullet = BulletManager.instance.GetPooledObject(transform.position, 7f, new Vector2(1, 1), OwnerType.MIDDLEBOSS2, 20f, null);

                    if (machineBulletCnt < 60)
                    {
                        machineAngle -= 3.6f;
                        Bullet.gameObject.transform.rotation = Quaternion.Euler(0, 0, machineAngle);
                    }
                    else
                    {
                        machineAngle += 3.6f;
                        Bullet.gameObject.transform.rotation = Quaternion.Euler(0, 0, machineAngle  );
                    }

                    AudioManager.instance.PlayOnShotSFX(6);

                    machineBulletCnt++;
                    machineTimer = 0;
                }

                if (machineBulletCnt == 130)
                    isMachine = false;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !isMachine)
            {
                animator.SetBool("isShoot", false);
                state = MiddleBossState.IDLE;
            }
        }

        if (isWait)
        {

            waitTimer += Time.deltaTime;
            if (waitTimer >= 1f)
            {
                if (state==MiddleBossState.LASER)
                    animator.SetBool("isLaser", true);
                if (state == MiddleBossState.GUN || state == MiddleBossState.MACHINEGUN)
                    animator.SetBool("isShoot", true);

                waitTimer = 0;
                isWait = false;
            }
        }
    }

    public void CreateLaser()
    {

        if (laserPattern == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject laser = Instantiate(laserPrefab);
                laser.transform.localScale = new Vector3(5, 3, 1);
                laser.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
                laser.transform.rotation = Quaternion.Euler(0, 0, laserAngle - (i - 1) * 30 - 270);
            }
        }

        if (laserPattern == 1)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject laser = Instantiate(laserPrefab);
                switch (i)
                {
                    case 0:
                        laser.transform.position = new Vector3(transform.position.x + 5.18f, transform.position.y, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, 90 + 270);
                        break;
                    case 1:
                        laser.transform.position = new Vector3(transform.position.x - 5.18f, transform.position.y, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, 180);
                        break;
                    case 2:
                        laser.transform.position = new Vector3(transform.position.x + 4.92f, transform.position.y - 4, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, 60 + 270);
                        break;
                    case 3:
                        laser.transform.position = new Vector3(transform.position.x - 4.92f, transform.position.y - 4, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, 120 - 270);
                        break;
                    case 4:
                        laser.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
                        break;

                }
            }
        }

        if (laserPattern == 2)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject laser = Instantiate(laserPrefab);
                switch(i)
                {
                    case 0:
                        laser.transform.position = new Vector3(transform.position.x + 5.18f, transform.position.y, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, 90 +270);
                        break;
                    case 1:
                        laser.transform.position = new Vector3(transform.position.x - 5.18f, transform.position.y, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, 180);
                        break;
                    case 2:
                        laser.transform.position = new Vector3(transform.position.x + 4.28f, transform.position.y-4, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, -50 + 270);
                        break;
                    case 3:
                        laser.transform.position = new Vector3(transform.position.x - 4.28f, transform.position.y - 4, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, -130 - 270);
                        break;
                    case 4:
                        laser.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
                        break;

                }
            }
        }

        if (laserPattern == 3)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject laser = Instantiate(laserPrefab);
                switch (i)
                {
                    case 0:
                        laser.transform.position = new Vector3(transform.position.x - 7.5f, transform.position.y - 5.29f, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, 270);
                        break;
                    case 1:
                        laser.transform.position = new Vector3(transform.position.x - 5f, transform.position.y - 5.29f, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, 270);
                        break;
                    case 2:
                        laser.transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y - 5.29f, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, 270);
                        break;
                    case 3:
                        laser.transform.position = new Vector3(transform.position.x + 7f, transform.position.y - 5.29f, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, 270);
                        break;
                    case 4:
                        laser.transform.position = new Vector3(transform.position.x + 4.5f, transform.position.y - 5.29f, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, 270);
                        break;
                    case 5:
                        laser.transform.position = new Vector3(transform.position.x + 2f, transform.position.y - 5.29f, transform.position.z);
                        laser.transform.rotation = Quaternion.Euler(0, 0, 270);
                        break;

                }
            }
        }

        Debug.Log("레이저");
        AudioManager.instance.PlayOnShotSFX(5);


        StartCoroutine("AddBoundary");

        StartCoroutine("AddLaser");
    }

    public void CreateBoundary()
    {
       
        
        if (laserPattern == 0)
        {

            for (int i = 0; i < 3; i++)
            {
                GameObject boundary = Instantiate(laserBoundaryPrefab);
                boundary.transform.localScale = new Vector3(3, 5, 1);
                boundary.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);

                Vector3 target_v = Player.transform.position - transform.position;
                laserAngle = Vector3.SignedAngle(transform.up, Player.transform.position - transform.position, transform.forward);
                boundary.transform.rotation = Quaternion.Euler(0, 0, laserAngle - (i - 1) * 30);
            }
        }


        if (laserPattern == 1)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject boundary = Instantiate(laserBoundaryPrefab);
                switch (i)
                {
                    case 0:
                        boundary.transform.position = new Vector3(transform.position.x + 5.18f, transform.position.y, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case 1:
                        boundary.transform.position = new Vector3(transform.position.x - 5.18f, transform.position.y, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case 2:
                        boundary.transform.position = new Vector3(transform.position.x + 4.92f, transform.position.y - 4, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 60);
                        break;
                    case 3:
                        boundary.transform.position = new Vector3(transform.position.x - 4.92f, transform.position.y - 4, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 120);
                        break;
                    case 4:
                        boundary.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
                        break;

                }
            }
        }

        if (laserPattern == 2)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject boundary = Instantiate(laserBoundaryPrefab);
                switch (i)
                {
                    case 0:
                        boundary.transform.position = new Vector3(transform.position.x + 5.18f, transform.position.y, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case 1:
                        boundary.transform.position = new Vector3(transform.position.x - 5.18f, transform.position.y, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case 2:
                        boundary.transform.position = new Vector3(transform.position.x + 4.28f, transform.position.y - 4, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, -50);
                        break;
                    case 3:
                        boundary.transform.position = new Vector3(transform.position.x - 4.28f, transform.position.y - 4, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, -130);
                        break;
                    case 4:
                        boundary.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
                        break;

                }
            }
        }

        if (laserPattern == 3)
        {
            for (int i = 0; i < 6; i++)
            {
                GameObject boundary = Instantiate(laserBoundaryPrefab);
                switch (i)
                {
                    case 0:
                        boundary.transform.position = new Vector3(transform.position.x - 7.5f, transform.position.y - 5.29f, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case 1:
                        boundary.transform.position = new Vector3(transform.position.x - 5f, transform.position.y - 5.29f, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case 2:
                        boundary.transform.position = new Vector3(transform.position.x - 2.5f, transform.position.y - 5.29f, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case 3:
                        boundary.transform.position = new Vector3(transform.position.x + 7f, transform.position.y - 5.29f, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case 4:
                        boundary.transform.position = new Vector3(transform.position.x + 4.5f, transform.position.y - 5.29f, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    case 5:
                        boundary.transform.position = new Vector3(transform.position.x + 2f, transform.position.y - 5.29f, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;

                }
            }
        }

    }

    public void Shoot()
    {
        if (state == MiddleBossState.GUN)
        {
            shootPattern = Random.Range(0, 3);

            AudioManager.instance.PlayOnShotSFX(7);

            for (int i = 0; i < 18; i++)
            {

                GameObject Bullet = BulletManager.instance.GetPooledObject(transform.position, 5f, new Vector2(1, 1), OwnerType.MIDDLEBOSS, 20f, null);
                Bullet.GetComponent<Bullet>().bulletAngle = i * 20;
                Bullet.gameObject.transform.rotation = Quaternion.Euler(0, 0, i * 20f);
            }

            StartCoroutine("AddShoot");
        }

        if (state == MiddleBossState.MACHINEGUN)
            isMachine = true;
    }



    void ChangePattern()
    {
        int randomStateNum = Random.Range(1, 4);

        while (randomStateNum == lastPattern)
            randomStateNum = Random.Range(1, 4);

        lastPattern = randomStateNum;

        state = (MiddleBossState)randomStateNum;

        if (state == MiddleBossState.LASER)
            laserPattern = Random.Range(0, 4);


    }

    IEnumerator AddBoundary()
    {
        yield return new WaitForSeconds(0.3f);

        if (laserPattern == 0)
        {

            for (int i = 0; i < 3; i++)
            {
                GameObject boundary = Instantiate(laserBoundaryPrefab);
                boundary.transform.localScale = new Vector3(3, 5, 1);
                boundary.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
                boundary.transform.rotation = Quaternion.Euler(0, 0, laserAngle - (i - 1) * 30 + 40);
            }
        }

        if (laserPattern == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject boundary = Instantiate(laserBoundaryPrefab);
                boundary.transform.position = new Vector3(transform.position.x - 0.09f + 2.5f * (i - 1), transform.position.y - 5.29f, transform.position.z);
            }
        }

        if (laserPattern == 2)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject boundary = Instantiate(laserBoundaryPrefab);
                switch (i)
                {
                    case 0:
                        boundary.transform.position = new Vector3(transform.position.x + 5.18f, transform.position.y, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case 1:
                        boundary.transform.position = new Vector3(transform.position.x - 5.18f, transform.position.y, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                    case 2:
                        boundary.transform.position = new Vector3(transform.position.x + 4.28f, transform.position.y - 4, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 50);
                        break;
                    case 3:
                        boundary.transform.position = new Vector3(transform.position.x - 4.28f, transform.position.y - 4, transform.position.z);
                        boundary.transform.rotation = Quaternion.Euler(0, 0, 130);
                        break;
                    case 4:
                        boundary.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
                        break;

                }
            }
        }

        if (laserPattern == 3)
        {
            GameObject boundary = Instantiate(laserBoundaryPrefab);
            boundary.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
        }


    }

    IEnumerator AddLaser()
    {
        yield return new WaitForSeconds(0.8f);

        if (laserPattern == 0)
        {

            for (int i = 0; i < 3; i++)
            {
                GameObject laser = Instantiate(laserPrefab);
                laser.transform.localScale = new Vector3(5, 3, 1);
                laser.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
                laser.transform.rotation = Quaternion.Euler(0, 0, laserAngle - (i - 1) * 30 + 310);
            }
        }

        if (laserPattern == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject laser = Instantiate(laserPrefab);
                laser.transform.position = new Vector3(transform.position.x - 0.09f + 2.5f * (i - 1), transform.position.y - 5.29f, transform.position.z);
            }
        }

        if (laserPattern == 2)
        {
                for (int i = 0; i < 5; i++)
                {
                    GameObject laser = Instantiate(laserPrefab);
                    switch (i)
                    {
                        case 0:
                            laser.transform.position = new Vector3(transform.position.x + 5.18f, transform.position.y, transform.position.z);
                            laser.transform.rotation = Quaternion.Euler(0, 0, 90 + 270);
                            break;
                        case 1:
                            laser.transform.position = new Vector3(transform.position.x - 5.18f, transform.position.y, transform.position.z);
                            laser.transform.rotation = Quaternion.Euler(0, 0, 180);
                            break;
                        case 2:
                            laser.transform.position = new Vector3(transform.position.x + 4.28f, transform.position.y - 4, transform.position.z);
                            laser.transform.rotation = Quaternion.Euler(0, 0, 50 + 270);
                            break;
                        case 3:
                            laser.transform.position = new Vector3(transform.position.x - 4.28f, transform.position.y - 4, transform.position.z);
                            laser.transform.rotation = Quaternion.Euler(0, 0, 130 - 270);
                            break;
                        case 4:
                            laser.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
                            break;

                    }
                }
        }

        if (laserPattern == 3)
        {
            GameObject laser = Instantiate(laserPrefab);
            laser.transform.position = new Vector3(transform.position.x - 0.09f, transform.position.y - 5.29f, transform.position.z);
            laser.transform.rotation = Quaternion.Euler(0, 0, 270);
        }

        AudioManager.instance.PlayOnShotSFX(5);

        Debug.Log("레이저");
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

    IEnumerator AddShoot()
    {
        yield return new WaitForSeconds(0.8f);
        shootPattern = Random.Range(0, 3);

        AudioManager.instance.PlayOnShotSFX(7);

        for (int i = 0; i < 18; i++)
        {
            GameObject Bullet = BulletManager.instance.GetPooledObject(transform.position, 5f, new Vector2(1, 1), OwnerType.MIDDLEBOSS, 20f, null);
            Bullet.GetComponent<Bullet>().bulletAngle = i * 30;
            Bullet.gameObject.transform.rotation = Quaternion.Euler(0, 0, i * 30f);
        }


    }

    IEnumerator BreakMagicStone()
    {
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("MagicStone").GetComponent<UnitInfo>().DecreaseHP(100000);
    }

}
