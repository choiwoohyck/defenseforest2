using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadComponent : MonoBehaviour
{
    // Start is called before the first frame update

    public Image deadBackGround;
    public Image playerDeadImage;
    public Image stoneDeadImage;


    public Camera mainCamera;
    public Sprite[] playerDeadImages;

    public Animator stoneAnimator;

    public TextMeshProUGUI RestartText;
    MoveController moveController;
    UnitInfo info;

    public bool fillBackgroundStart = false;
    public bool isPlayerDead = true;
    public bool alreadyWork = false;
    bool isAnimFinsh = false;

    float fillBackgroundTimer = 0;

    void Start()
    {
        moveController = GetComponent<MoveController>();    
        info = GetComponent<UnitInfo>();
        isPlayerDead = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!fillBackgroundStart) return;

        if (isPlayerDead)
            isAnimFinsh = true;

        else if (!isPlayerDead && stoneAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && stoneAnimator.GetCurrentAnimatorStateInfo(0).IsName("dead"))
        {
            isAnimFinsh = true;
            GameObject.Find("MagicStone").transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }

        if (isAnimFinsh)
        {
            if (mainCamera.orthographicSize >= 3)
                mainCamera.orthographicSize -= Time.deltaTime * 2f;
            
            fillBackgroundTimer += Time.deltaTime;
            deadBackGround.fillAmount = fillBackgroundTimer / 2;

            if (deadBackGround.fillAmount >= 1)
            {
                RestartText.gameObject.SetActive(true);
                if (isPlayerDead)
                    playerDeadImage.gameObject.SetActive(true);
                else
                    stoneDeadImage.gameObject.SetActive(true);

                Time.timeScale = 0;
            }

        }

    }

    public void InitSetting()
    {
        if (isPlayerDead)
        {
            Animator animaotr = GetComponent<Animator>();
            animaotr.SetBool("isDead", true);

            float xDir = animaotr.GetFloat("xDir");
            float yDir = animaotr.GetFloat("yDir");
            Vector3 dir = moveController.moveDir;

            SpriteRenderer renderer = playerDeadImage.GetComponent<SpriteRenderer>();

            if (dir.x != 0 && dir.y != 0)
            {
                xDir = dir.x;
                yDir = dir.y;
            }

            if ((int)xDir == 1)
                playerDeadImage.sprite = playerDeadImages[0];
            else if ((int)xDir == -1)
                playerDeadImage.sprite = playerDeadImages[2];

            else if ((int)yDir == 1)
                playerDeadImage.sprite = playerDeadImages[1];
            else if ((int)yDir == -1)
                playerDeadImage.sprite = playerDeadImages[3];


            Debug.Log("플레이어 뒤짐2");

        }

        else
        {
            mainCamera.GetComponent<CameraController>().isFollow = false;
            stoneAnimator.SetBool("isDead", true);
            mainCamera.transform.position = GameObject.Find("MagicStone").transform.position;
        }

        AudioManager.instance.StopBGM();
        deadBackGround.gameObject.SetActive(true);
        fillBackgroundStart = true;
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
 