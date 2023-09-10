using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowBuildButton : MonoBehaviour
{
    // Start is called before the first frame update

    public Button BuildUIButton;
    public Button FireElementButton;
    public Button IceElementButton;
    public Button LeafElementButton;
    public Button StoneElementButton;


    public Image SelectBorder;


    RectTransform rectTransform;
    RectTransform buttonRectTransform;

    
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

        StartCoroutine("BuildUIUpDown");

        Debug.Log(rectTransform.position.y);
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

    void ElementButtonClick(int order)
    {

        switch(order)
        {
            case 0:
                SelectBorder.rectTransform.anchoredPosition = new Vector2(-371.2f, -18.4f);
                break;
            case 1:
                SelectBorder.rectTransform.anchoredPosition = new Vector2(-289.2f, -18.4f);
                break;

            case 2:
                SelectBorder.rectTransform.anchoredPosition = new Vector2(-205.2f, -18.4f);
                break;

            case 3:
                SelectBorder.rectTransform.anchoredPosition = new Vector2(-129.2f, -18.4f);
                break;

        }


    }
}
