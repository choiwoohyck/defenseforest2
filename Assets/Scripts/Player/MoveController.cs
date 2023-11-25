using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
using Unity.VisualScripting;

public class MoveController : MonoBehaviour
{

    public float movementSpeed = 2.5f;
    float shootTime = 0;
    public float teleportTimer = 1.0f;
    public float maxTeleportTimer = 1.0f;

    public bool inMiddleBossStage = false;
    bool isTeleport = false;

    Vector2 movement = new Vector2();
    public Vector2 moveDir = new Vector2();
    Rigidbody2D rigidbody2D;

    Animator animator;
    UnitInfo info;

    public bool isStop = false;
    public bool isWASDControl = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        info = GetComponent<UnitInfo>();
        Time.timeScale = 1f;


    }

    // Update is called once per frame
    void Update()
    {
        if (info.hp > 0 && !isStop && GameManager.instance.isTutorialFinished)
        {
            animator.speed = 1;
            rigidbody2D.velocity = Vector3.zero;
            rigidbody2D.angularVelocity = 0;
            teleportTimer += Time.deltaTime;
            UpdateState();
            Shoot();

            if (Input.GetMouseButtonDown(0))
            {
                ClickElement();
            }

            //if (Input.GetKey(KeyCode.F1))
            //    StageManager.instance.gameTime = 120;

            //if (Input.GetKey(KeyCode.F2))
            //    info.hp = 100000;


        }

        else
            animator.speed = 0;

        if (isStop)
        {
            animator.SetFloat("yDir", 1f);
        }
        
    }

    private void FixedUpdate()
    {
        if (info.hp > 0 && !isStop && GameManager.instance.isTutorialFinished)
        {

            MoveCharacter();
            Teleport();
        }
    }

    private void MoveCharacter()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isWASDControl = !isWASDControl;
            UIManager.instance.WASDText.SetActive(isWASDControl);
            UIManager.instance.ArrowKeyText.SetActive(!isWASDControl);

        }

        if (isWASDControl)
        {

            if (Input.GetKey(KeyCode.W))
            {
                moveDir.y = 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                moveDir.x = -1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                moveDir.y = -1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                moveDir.x = 1;
            }


            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                AudioManager.instance.PlayOnShotWALKSFX();
                moveDir.Normalize();
            }

            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                moveDir.y = 0;
            }

            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                moveDir.x = 0;
            }
        }

        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveDir.y = 1;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveDir.x = -1;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                moveDir.y = -1;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveDir.x = 1;
            }


            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                AudioManager.instance.PlayOnShotWALKSFX();
                moveDir.Normalize();
            }

            if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
            {
                moveDir.y = 0;
            }

            if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                moveDir.x = 0;
            }
        }

   

        rigidbody2D.MovePosition(rigidbody2D.position + moveDir * movementSpeed * Time.fixedDeltaTime);

        float xPos = transform.position.x;
        float yPos = transform.position.y;

        if (!inMiddleBossStage)
        {
            xPos = Mathf.Clamp(xPos, -12.8f, 12.8f);
            yPos = Mathf.Clamp(yPos, -2.5f, 15.4f);

        }
        else
        {
            xPos = Mathf.Clamp(xPos, -8.93f, 8.44f);
            yPos = Mathf.Clamp(yPos, 5.2f, 14.45f);
        }

        transform.position = new Vector3(xPos, yPos, transform.position.z);

    }

    private void ClickElement()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        
        if (hit.transform.gameObject.tag == "Element" && hit.transform.gameObject.GetComponent<Element>().elementType != ElementType.STONE)
        {
            if (hit.transform.gameObject.GetComponent<Element>().elementType == ElementType.FIRE && hit.transform.gameObject.GetComponent<Element>().isRoad) return;

            GameObject selectElement = hit.transform.gameObject;
            selectElement.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = !selectElement.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled;
            //selectElement.transform.GetChild(0).gameObject.SetActive(!selectElement.transform.GetChild(0).gameObject.activeSelf);            
        }
    }

    private void Teleport()
    {
        if (isWASDControl)
        {

            if (Input.GetKey(KeyCode.W))
            {
                moveDir.y = 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                moveDir.x = -1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                moveDir.y = -1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                moveDir.x = 1;
            }


            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                moveDir.Normalize();
            }

            else
            {
                moveDir = new Vector2(0, 0);
            }
        }

        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                moveDir.y = 1;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveDir.x = -1;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                moveDir.y = -1;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveDir.x = 1;
            }


            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                moveDir.Normalize();
            }

            else
            {
                moveDir = new Vector2(0, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && teleportTimer >= maxTeleportTimer)
        {
            
            GetComponent<UnitInfo>().isInvincible = true;
            teleportTimer = 0;

            float xPos = transform.position.x + moveDir.x;
            float yPos = transform.position.y + moveDir.y;

            xPos = Mathf.Clamp(xPos, -12.8f, 12.8f);
            yPos = Mathf.Clamp(yPos, -2.5f, 15.4f);

            transform.position = new Vector3(xPos, yPos, transform.position.z);
            EffectManager.instance.CreateEffect(EffectType.TELEPORT, transform.position, transform.rotation);
        }

       
    }

    private void Shoot()
    {
        shootTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && shootTime >= gameObject.GetComponent<UnitInfo>().attackDelay)
        {

            Vector2 rotVec = new Vector2(animator.GetFloat("xDir"), animator.GetFloat("yDir"));
            Vector2 startVec = new Vector2(transform.position.x, transform.position.y);


            GameObject Bullet =  BulletManager.instance.GetPooledObject(startVec, 10f, rotVec, OwnerType.PLAYER,PlayerInfo.Instance.playerDamage,null);
            AudioManager.instance.PlayOnShotSFX(0);
            shootTime = 0;
        }

    }

    private void UpdateState()
    {
        if (Mathf.Approximately(moveDir.x , 0) && Mathf.Approximately(moveDir.y , 0))
        {
            animator.SetBool("isMove", false);
        }

        else
        {
            animator.SetBool("isMove", true);
            animator.SetFloat("xDir", moveDir.x);
            animator.SetFloat("yDir", moveDir.y);
        }

       
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //rigidbody2D.velocity = Vector2.zero;
    }
}
