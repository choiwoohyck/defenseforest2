using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BattlePopUp : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject battlePopUpBackground;
    public Image yesButton;
    public Image noButton;

    public Sprite yesFocusButtonSprite;
    public Sprite noFocusButtonSprite;

    public Sprite yesButtonSprite;
    public Sprite noButtonSprite;

    public GameObject battlePopUpUI;
    public GameObject battlePopUpButton;
    public GameObject buildUI;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveBackground(bool active)
    {
        battlePopUpBackground.SetActive(active);
    }


    public void OnMouseYesButton()
    {
        yesButton.sprite = yesFocusButtonSprite;
    }

    public void ExitMouseYesButton()
    {
        yesButton.sprite = yesButtonSprite;
    }

    public void OnMouseNoButton()
    {
        noButton.sprite = noFocusButtonSprite;
    }

    public void ExitMouseNoButton()
    {
        noButton.sprite = noButtonSprite;
    }


    public void ClickNoButton()
    {
        ActiveBackground(false);
        battlePopUpUI.SetActive(false);
        noButton.sprite = noButtonSprite;
        AudioManager.instance.PlayOnShotSFX(2);
    }

    public void ClickBattleButton()
    {
        if (UIManager.instance.SettingUI.activeSelf || !GameManager.instance.isTutorialFinished || AllyUnitManager.instance.alreadyClick) return;
        
        if (GameManager.instance.Stage != 6)
        {
            ActiveBackground(true);
            battlePopUpUI.SetActive(true);
            AudioManager.instance.PlayOnShotSFX(2);
        }
        else
        {
            EndingManager.instance.isEnding = true;
            AudioManager.instance.PlayOnShotSFX(2);

            if (buildUI.GetComponent<ShowBuildButton>().up)
                buildUI.GetComponent<ShowBuildButton>().BuildButtonClick();

            buildUI.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ClickYesButton()
    {
        ActiveBackground(false);
        battlePopUpUI.SetActive(false);
        yesButton.sprite = yesButtonSprite;

        AudioManager.instance.PlayOnShotSFX(2);
        if(buildUI.GetComponent<ShowBuildButton>().up)
            buildUI.GetComponent<ShowBuildButton>().BuildButtonClick();

        buildUI.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        UIManager.instance.fadeImg.gameObject.SetActive(true);
        if (GameManager.instance.Stage == 2 || GameManager.instance.Stage == 5)
        {
            UIManager.instance.elementOffText.gameObject.SetActive(true);
            AllyUnitManager.instance.AllInactive();
        }
        UIManager.instance.changeTimerOn = true;
        battlePopUpButton.SetActive(false);
        AudioManager.instance.StopBGM();
    }


    public void OnMouseBattleButton()
    {
        if (!GameManager.instance.isTutorialFinished || AllyUnitManager.instance.alreadyClick) return;
        

        battlePopUpButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void ExitMouseBattleButton()
    {
        battlePopUpButton.transform.localScale = new Vector3(1f, 1f, 1f);
    }

}
