using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Image coolTimeImage;
    public MoveController player;
    void Start()
    {
        coolTimeImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        coolTimeImage.fillAmount = player.teleportTimer / player.maxTeleportTimer;

        if (player.teleportTimer >= 1)
        {
            Color originColor = coolTimeImage.color;
            originColor.a = 0;
            coolTimeImage.color = originColor;
        }
        else
        {
            Color originColor = coolTimeImage.color;
            originColor.a = 1;
            coolTimeImage.color = originColor;
        }
    }
}
