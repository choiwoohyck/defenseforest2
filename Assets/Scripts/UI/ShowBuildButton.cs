using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowBuildButton : MonoBehaviour
{
    // Start is called before the first frame update

    [Header ("���� ��ư��")]
    public Button BuildUIButton;
    public Button FireElementButton;
    public Button IceElementButton;
    public Button LeafElementButton;
    public Button StoneElementButton;


    public Image SelectBorder;


    RectTransform rectTransform;
    RectTransform buttonRectTransform;

    [Header("���� UI")]
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
        DescriptionName.text = "�Ҳ� ����";
        DescriptionImage.sprite = DescriptionImages[0];
        DescriptionDamage.text = "20";
        DescriptionHP.text = "100";
        Description1.text = "�Ҳ��� ��� ���� �����մϴ�.";
        Description2.text = "ü���� ���ϸ� �����մϴ�.";
    }

    public void OnMouseEnterICEButton()
    {
        if (UIManager.instance.SettingUI.activeSelf || !GameManager.instance.isTutorialFinished || !up) return;

        DescriptionUI.SetActive(true);
        DescriptionName.text = "���� ����";
        DescriptionImage.sprite = DescriptionImages[1];
        DescriptionDamage.text = "20";
        DescriptionHP.text = "100";
        Description1.text = "���� �̻� �����ϸ� ���� �󸳴ϴ�.";
        Description2.text = "���� �����ϴ� ����â�� �����ϴ�.";
    }

    public void OnMouseEnterLeafButton()
    {
        if (UIManager.instance.SettingUI.activeSelf || !GameManager.instance.isTutorialFinished || !up) return;

        DescriptionUI.SetActive(true);
        DescriptionName.text = "Ǯ ����";
        DescriptionImage.sprite = DescriptionImages[2];
        DescriptionDamage.text = "50";
        DescriptionHP.text = "100";
        Description1.text = "������ �����մϴ�..";
        Description2.text = "���Ȱ��� ��ġ�ϰ� �����մϴ�.";
    }

    public void OnMouseEnterStoneButton()
    {
        if (UIManager.instance.SettingUI.activeSelf || !GameManager.instance.isTutorialFinished || !up) return;

        DescriptionUI.SetActive(true);
        DescriptionName.text = "���� ����";
        DescriptionImage.sprite = DescriptionImages[3];
        DescriptionDamage.text = "0";
        DescriptionHP.text = "0";
        Description1.text = "�÷��� �Ͽ� ������ ���ϴ� ��ſ�";
        Description2.text = "5�ʿ� 15�������� ȹ���մϴ�.";
    }

    public void ExitMouseBuildButton()
    {
        DescriptionUI.SetActive(false);
    }

}
