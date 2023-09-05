using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MagicStoneHPBar : MonoBehaviour
{
    // Start is called before the first frame update

    private Image hpBarImage;
    public UnitInfo stoneInfo;

    void Start()
    {
        hpBarImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        hpBarImage.fillAmount = stoneInfo.hp / stoneInfo.maxHp;
    }
}
