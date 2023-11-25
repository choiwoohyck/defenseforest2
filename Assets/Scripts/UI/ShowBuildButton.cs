using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowBuildButton : MonoBehaviour
{
    // Start is called before the first frame update

    [Header ("생성 버튼들")]
    public Button BuildUIButton;
    public Button FireElementButton;
    public Button IceElementButton;
    public Button LeafElementButton;
    public Button StoneElementButton;


    public Image SelectBorder;


    RectTransform rectTransform;
    RectTransform buttonRectTransform;

    [Header("설명 UI")]
    public GameObject DescriptionUI;
    public TextMeshProUGUI DescriptionName;
    public Image DescriptionImage;
    public TextMeshProUGUI DescriptionDamage;
    public TextMeshProUGUI DescriptionHP;
    public TextMeshProUGUI Description1;
    public TextMeshProUGUI Description2;
    public Sprite[] DescriptionImages;



    public bool up = true;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        buttonRectTransform = BuildUIButton.GetComponent<RectTransform>();

        FireElementButton.onClick.AddListener(() => ElementButtonClick(0));
        IceElementButton.onClick.AddListener(() => ElementButtonClick(1));
        LeafElementButton.onClick.AddListener(() => ElementButtonClick(2));
        StoneElementButton.onClick.AddListener(() => ElementButtonClick(3));


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildButtonClick()
    {
        if (GameManager.instance.inActiveBuildButton) return;
        if (UIManager.instance.SettingUI.activeSelf) return;

        else
        AudioManager.instance.PlayOnShotSFX(2);

        StartCoroutine("BuildUIUpDown");

        Debug.Log(rectTransform.position.y);
    }

    public void OnMouseBuildButton()
    {

    }



    IEnumerator BuildUIUpDown()
    {
        up = !up;
        AllyUnitManager.instance.isBuild = false;
        while (true)
        {
            float y = rectTransform.anchoredPosition.y;

            if (up) 
            {
                y += Time.deltaTime * 200f;
                rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, y);

                if (y >= -470)
                    break;
            }

            else
            {
                y -= Time.deltaTime * 200f;

                if (y <= -578)
                    break;
            }

            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, y);

            yield return null;

            

        }

        if (up)
            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, -470);
        else
            rectTransform.anchoredPosition = new Vector3(rectTransform.anchoredPosition.x, -578);

        AllyUnitManager.instance.isBuild = true;

    }

    public void ElementButtonClick(int order)
    {
        if (UIManager.instance.SettingUI.activeSelf || !GameManager.instance.isTutorialFinished) return;


        AudioManager.instance.PlayOnShotSFX(2);

        if (GameManager.instance.inActiveBuildButton) return;

        switch (order)
        {
            case 0:
                SelectBorder.rectTransform.anchoredPosition = new Vector2(-374.1f, -18.4f);
                break;
            case 1:
                SelectBorder.rectTransform.anchoredPosition = new Vector2(-290.4f, -18.4f);
                break;

            case 2:
                SelectBorder.rectTransform.anchoredPosition = new Vector2(-204.2f, -18.4f);
                break;

            case 3:
                SelectBorder.rectTransform.anchoredPosition = new Vector2(-118.8f, -18.4f);
                break;

        }

    }

    public void OnMouseEnterFireButton()
    {
        if (UIManager.instance.SettingUI.activeSelf || !GameManager.instance.isTutorialFinished || !up) return;

        DescriptionUI.SetActive(true);
        DescriptionName.text = "불꽃 정령";
        DescriptionImage.sprite = DescriptionImages[0];
        DescriptionDamage.text = "20";
        DescriptionHP.text = "100";
        Description1.text = "불꽃을 뱉어 적을 공격합니다.";
        Description2.text = "체력이 다하면 폭발합니다.";
    }

    public void OnMouseEnterICEButton()
    {
        if (UIManager.instance.SettingUI.activeSelf || !GameManager.instance.isTutorialFinished || !up) return;

        DescriptionUI.SetActive(true);
        DescriptionName.text = "얼음 정령";
        DescriptionImage.sprite = DescriptionImages[1];
        DescriptionDamage.text = "20";
        DescriptionHP.text = "100";
        Description1.text = "일정 이상 공격하면 적을 얼립니다.";
        Description2.text = "적을 관통하는 얼음창을 날립니다.";
    }

    public void OnMouseEnterLeafButton()
    {
        if (UIManager.instance.SettingUI.activeSelf || !GameManager.instance.isTutorialFinished || !up) return;

        DescriptionUI.SetActive(true);
        DescriptionName.text = "풀 정령";
        DescriptionImage.sprite = DescriptionImages[2];
        DescriptionDamage.text = "50";
        DescriptionHP.text = "100";
        Description1.text = "적에게 돌진합니다..";
        Description2.text = "독안개를 설치하고 공격합니다.";
    }

    public void OnMouseEnterStoneButton()
    {
        if (UIManager.instance.SettingUI.activeSelf || !GameManager.instance.isTutorialFinished || !up) return;

        DescriptionUI.SetActive(true);
        DescriptionName.text = "바위 정령";
        DescriptionImage.sprite = DescriptionImages[3];
        DescriptionDamage.text = "0";
        DescriptionHP.text = "0";
        Description1.text = "플레이 턴에 공격을 못하는 대신에";
        Description2.text = "5초에 15에너지를 획득합니다.";
    }

    public void ExitMouseBuildButton()
    {
        DescriptionUI.SetActive(false);
    }

}
