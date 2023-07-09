using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
public class MoveController : MonoBehaviour
{

    public float movementSpeed = 2.5f;
    float shootTime = 0;
    Vector2 movement = new Vector2();
    Vector2 moveDir = new Vector2();
    Rigidbody2D rigidbody2D;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody2D.velocity = Vector3.zero;
        rigidbody2D.angularVelocity = 0;

        UpdateState();
        Shoot();

        if (Input.GetMouseButtonDown(0))
        {
            ClickElement();
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {


        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) 
        {
            moveDir.x = Input.GetAxisRaw("Horizontal");
            moveDir.y = Input.GetAxisRaw("Vertical");

            moveDir.Normalize();
        }



        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

       
        movement.Normalize();

        float xPos = transform.position.x;
        float yPos = transform.position.y;

        xPos = Mathf.Clamp(xPos, -12.8f, 12.8f);
        yPos = Mathf.Clamp(yPos, -2.5f, 15.4f);

        rigidbody2D.MovePosition(rigidbody2D.position + movement * movementSpeed * Time.fixedDeltaTime);
        transform.position = new Vector3(xPos, yPos, transform.position.z);

    }

    private void ClickElement()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);


        if (hit.transform.gameObject.tag == "Element")
        {
            GameObject selectElement = hit.transform.gameObject;
            selectElement.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = !selectElement.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled;
            //selectElement.transform.GetChild(0).gameObject.SetActive(!selectElement.transform.GetChild(0).gameObject.activeSelf);            
        }
    }

    private void Shoot()
    {
        shootTime += Time.deltaTime;

        if (Input.GetKey(KeyCode.Space) && shootTime >= gameObject.GetComponent<UnitInfo>().attackDelay)
        {

            Vector2 rotVec = new Vector2(moveDir.x, moveDir.y);
            Vector2 startVec = new Vector2(transform.position.x, transform.position.y);


            BulletManager.instance.GetObject(startVec, 10f, rotVec, OwnerType.PLAYER,PlayerInfo.Instance.playerDamage);

            shootTime = 0;
        }

    }

    private void UpdateState()
    {
        if (Mathf.Approximately(movement.x , 0) && Mathf.Approximately(movement.y , 0))
        {
            animator.SetBool("isMove", false);
        }

        else
        {
            animator.SetBool("isMove", true);
        }

        animator.SetFloat("xDir", moveDir.x);
        animator.SetFloat("yDir", moveDir.y);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        rigidbody2D.velocity = Vector2.zero;
    }
}
