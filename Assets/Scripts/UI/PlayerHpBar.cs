using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHpBar : MonoBehaviour
{
    private Image hpBarImage;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        hpBarImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        hpBarImage.fillAmount = Player.GetComponent<UnitInfo>().hp / Player.GetComponent<UnitInfo>().maxHp;
    }
}
