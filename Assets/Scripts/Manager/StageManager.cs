using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StageManager : MonoBehaviour
{
    [HideInInspector]
    public static StageManager instance;

    public GameObject GlobalLight;


    public float gameTime = 0;
    public bool isClearSoundPlay = false;
    public bool bossKill = false;
    
    // Start is called before the first frame update

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
        if (GameManager.instance.isGameTurn)
        {
            float changeSpeed = GameManager.instance.Stage == 3 || GameManager.instance.Stage == 6 ? 2f:4f;
            
            float color = (GlobalLight.GetComponent<Light2D>().color.r * 255) + Time.deltaTime * changeSpeed;
            
            GlobalLight.GetComponent<Light2D>().color = new Color(color / 255f, color / 255f, color / 255f);
            gameTime += Time.deltaTime;
        }

     

        if ((GameManager.instance.Stage != 3 && GameManager.instance.Stage != 6) && (gameTime >= GameManager.instance.gameMaxTime) || bossKill)
        {
            
            GameManager.instance.isGameTurn = false;
            AudioManager.instance.StopBGM();
            if (!isClearSoundPlay)
            {
                AudioManager.instance.PlayOnShotSFX(1);
                isClearSoundPlay = true;
            }

            StartCoroutine("ClearUIOpen");
            ChangeDay();

            gameTime = 0;
            bossKill = false;
            
        }

        else if (gameTime >= GameManager.instance.gameMaxTime)
        {
            if(GameManager.instance.Stage == 3)
                GameManager.instance.middleBossKillFail = true;
            else
                GameManager.instance.finalBossKillFail = true;

        }


    }

    public void ChangeNight()
    {
        GlobalLight.GetComponent<Light2D>().color = new Color(15 / 255f, 15 / 255f, 15 / 255f);
    }

    public void ChangeDay()
    {
        GameManager.instance.spawner.stop = true;

        foreach (var enemy in EnemyUnitManager.instance.enemyUnits)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemy>().noEnergy = true;
                enemy.GetComponent<Enemy>().hp = 0;
            }
        }

        GlobalLight.GetComponent<Light2D>().color = new Color(1, 1, 1);
        GameManager.instance.inActiveBuildButton = false;
        UIManager.instance.DayBarImage.gameObject.SetActive(false);
        UIManager.instance.buildButton.SetActive(true);

        if (!UIManager.instance.BuildPopupUI.GetComponent<ShowBuildButton>().up)
        {
            UIManager.instance.BuildPopupUI.GetComponent<ShowBuildButton>().BuildButtonClick();
        }

        UIManager.instance.battleButton.SetActive(true);

        if (GameManager.instance.Stage == 3)
            AllyUnitManager.instance.AllInactive(true);

    }

    IEnumerator ClearUIOpen()
    {
        UIManager.instance.clearUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        isClearSoundPlay = false;
        UIManager.instance.clearUI.SetActive(false);
        AudioManager.instance.ChangeBGM(0);

    }
}