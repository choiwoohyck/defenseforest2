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

    public Camera mainCamera;
    public Sprite[] playerDeadImages;
    public TextMeshProUGUI RestartText;
    MoveController moveController;
    UnitInfo info;

    public bool fillBackgroundStart = false;

    float fillBackgroundTimer = 0;

    void Start()
    {
        moveController = GetComponent<MoveController>();    
        info = GetComponent<UnitInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!fillBackgroundStart) return;

        if (mainCamera.orthographicSize >= 3)
            mainCamera.orthographicSize -= Time.deltaTime * 2f;
        
          

        fillBackgroundTimer += Time.deltaTime;
        deadBackGround.fillAmount = fillBackgroundTimer / 0.5f;

        if (deadBackGround.fillAmount >= 1)
            RestartText.gameObject.SetActive(true);
    }

    public void InitSetting()
    {
        Animator animaotr = GetComponent<Animator>();
        animaotr.SetBool("isDead", true);
        AudioManager.instance.StopBGM();

        deadBackGround.gameObject.SetActive(true);
        playerDeadImage.gameObject.SetActive(true);

        AllyUnitManager.instance.allyUnits.Remove(gameObject);

        fillBackgroundStart = true;

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


    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
 