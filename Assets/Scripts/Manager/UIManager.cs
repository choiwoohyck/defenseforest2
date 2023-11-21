using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector]
    public static UIManager instance;

    public GameObject energyText;
    public GameObject buildButton;
    public GameObject battleButton;
    public GameObject SettingUI;

    public Image fadeImg; // fade에 쓸 이미지
    public Image DayBarFillImage;
    public Image DayBarImage;
    public float fadeTime; //화면이 변할 시간
    public bool fadeout;

    public GameObject BuildPopupUI;
    public GameObject clearUI;
    public GameObject elementOffText;
    public GameObject MiddleBoss;
    public GameObject FinalBoss;

    public GameObject killFailBossText;

    public GameObject WASDText;
    public GameObject ArrowKeyText;

    float changeTimer = 0f;

    public bool changeTimerOn = false;
    public bool fadein;

    private void Awake() 
    {
        instance = this;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.Stage == 6)
        {
            battleButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Ending";
        }

        if (changeTimerOn)
        {
            fadein = true;
            GameManager.instance.inActiveBuildButton = true;
        }

        if (fadein)
        {
            Color tmpColor = fadeImg.color;
            tmpColor.a  += Time.deltaTime * 2f;
            fadeImg.color = tmpColor;

            if (fadeImg.color.a >= 1)
            {
                fadein = false;
                changeTimerOn = true;
                StageManager.instance.ChangeNight();
            }
            
        }

        if (changeTimerOn)
        {
            changeTimer += Time.deltaTime;
            if (changeTimer >= 1f)
            {
                fadeout = true;
                changeTimer = 0;
                changeTimerOn = false;
                GameManager.instance.isGameTurn = true;
            }
        }

        if (fadeout)
        {
            Color tmpColor = fadeImg.color;
            tmpColor.a -= Time.deltaTime;
            fadeImg.color = tmpColor;

            //if (GameManager.instance.Stage == 2)
            //{
            //    Color tmpColor2 = elementOffText.GetComponent<TextMeshProUGUI>().color   ;
            //    tmpColor2.a -= Time.deltaTime;
            //    elementOffText.GetComponent<TextMeshPro>().color = tmpColor2;
            //}

            if (fadeImg.color.a <= 0)
            {
                fadeout = false;
                fadeImg.gameObject.SetActive(false);

                GameManager.instance.Stage++;

                if (GameManager.instance.Stage == 3 || GameManager.instance.Stage == 6)
                {
                    if (GameManager.instance.Stage == 3)
                        MiddleBoss.SetActive(true);
                    else
                        FinalBoss.SetActive(true);
                    elementOffText.gameObject.SetActive(false);

                    GameManager.instance.gameMaxTime = 120;
                }
                else
                {
                    GameManager.instance.spawner.stop = false;
                    GameManager.instance.gameMaxTime = 60;
                }


                AudioManager.instance.ChangeBGM(1);
            }
        }

        if (GameManager.instance.isGameTurn)
        {
            DayBarImage.gameObject.SetActive(true);
            DayBarFillImage.fillAmount = (GameManager.instance.gameMaxTime - StageManager.instance.gameTime) / GameManager.instance.gameMaxTime;
        }

        energyText.GetComponent<TextMeshProUGUI>().text = GameManager.instance.energy.ToString();

        GameManager.instance.inActiveBuildButton = SettingUI.activeSelf;
    }

    

    public IEnumerator ClearUIOpen()
    {
        AudioManager.instance.StopBGM();
        clearUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        clearUI.SetActive(false);
    }

    public void ClickQuitButton()
    {
        SettingUI.SetActive(false);
        AudioManager.instance.PlayOnShotSFX(2);
         
        Time.timeScale = 1f;
    }

    public void  ClickPauseButton()
    {
        if (AllyUnitManager.instance.alreadyClick) return;
        SettingUI.SetActive(!SettingUI.activeSelf);
        Time.timeScale = Time.timeScale == 0f ? 1.0f : 0f;
    }

    public void ClickMainButton()
    {
        SceneManager.LoadScene("MainScene");    
    }
}
