using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    [HideInInspector]
    public static UIManager instance;

    public GameObject energyText;
    public GameObject buildButton;
    public GameObject battleButton;

    public Image fadeImg; // fade에 쓸 이미지
    public Image DayBarFillImage;
    public Image DayBarImage;
    public float fadeTime; //화면이 변할 시간
    public bool fadeout;

    public GameObject BuildPopupUI;
    public GameObject clearUI;

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

            if (fadeImg.color.a <= 0)
            {
                fadeout = false;
                fadeImg.gameObject.SetActive(false);
                GameManager.instance.spawner.stop = false;
                GameManager.instance.Stage++;
                AudioManager.instance.ChangeBGM(1);
            }
        }

        if (GameManager.instance.isGameTurn)
        {
            DayBarImage.gameObject.SetActive(true);
            DayBarFillImage.fillAmount = (60 - StageManager.instance.gameTime) / 60;
        }

        energyText.GetComponent<TextMeshProUGUI>().text = GameManager.instance.energy.ToString();
    }

    

    public IEnumerator ClearUIOpen()
    {
        AudioManager.instance.StopBGM();
        clearUI.SetActive(true);
        yield return new WaitForSeconds(3f);
        clearUI.SetActive(false);
    }
}
