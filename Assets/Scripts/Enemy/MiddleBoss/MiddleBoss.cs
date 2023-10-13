using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
using UnityEngine.Rendering;

public class MiddleBoss : MonoBehaviour
{
    MiddleBossState state;
    UnitInfo info;

    public GameObject laserPrefab;
    public GameObject laserBoundaryPrefab;

    GameObject Player;
    Animator animator;

    float patternTimer = 0;
    float waitTimer = 0;
    float laserAngle = 0;

    public float patterCoolDownTime = 2f;

    bool isWait = false;

    int laserPattern = 0;

    // Start is called before the first frame update

    private void Awake()
    {
       info = GetComponent<UnitInfo>();
       state = MiddleBossState.IDLE;
    }

    void init()
    {
        info.StatusInit(1000, 100, 20);
    }

    

    void Start()
    {
        Player = GameObject.Find("player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == MiddleBossState.IDLE)
        {
            
            patternTimer += Time.deltaTime;
            

            if (patternTimer >= patterCoolDownTime)
            {
                
                ChangePattern();

                if (state == MiddleBossState.LASER)
                    CreateBoundary();

                isWait = true;
                patternTimer = 0;
            }
        }

        if (state == MiddleBossState.LASER && !isWait)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
            {
                animator.SetBool("isLaser", false);
                state = MiddleBossState.IDLE;
            }
        }

        if (isWait)
        {

            waitTimer += Time.deltaTime;
            if (waitTimer >= 1f)
            {
                animator.SetBool("isLaser", true);
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
                laser.transform.localScale = new Vector3(3, 3, 1);
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
                boundary.transform.localScale = new Vector3(3, 3, 1);
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

    void ChangePattern()
    {
        int randomStateNum = Random.Range(0, 2);
        //state = (MiddleBossState)randomStateNum;
        state = MiddleBossState.LASER;

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
                boundary.transform.localScale = new Vector3(3, 3, 1);
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
                laser.transform.localScale = new Vector3(3, 3, 1);
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


    }

    

}
