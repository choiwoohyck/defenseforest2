using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject SettingWindow;
    public GameObject startButton;
    public GameObject settingButton;
    public GameObject quitButton;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickSettingButton()
    {
        AudioManager.instance.PlayOnShotSFX(2);
        SettingWindow.SetActive(true);
    }

    public void ClickSettingQuitButton()
    {
        AudioManager.instance.PlayOnShotSFX(2);

        SettingWindow.SetActive(false);
    }

    public void ClickStartButton()
    {
        AudioManager.instance.PlayOnShotSFX(2);

        SceneManager.LoadScene("SampleScene");
    }

    public void OnStartButton()
    {
        startButton.transform.localScale = new Vector3(0.9f, 0.7f, 1f);
        startButton.GetComponent<Image>().color = new Color(1, 1, 1);
    }

    public void OnSettingButton()
    {
        settingButton.transform.localScale = new Vector3(0.9f, 0.7f, 1f);
        settingButton.GetComponent<Image>().color = new Color(1, 1, 1);
    }

    public void OnQuitButton()
    {
        quitButton.transform.localScale = new Vector3(0.9f, 0.7f, 1f);
        quitButton.GetComponent<Image>().color = new Color(1, 1, 1);
    }

    public void ExitStartButton()
    {
        startButton.transform.localScale = new Vector3(0.7f, 0.5f, 1.0f);
        startButton.GetComponent<Image>().color = new Color(224f/255f, 224f/255f, 224f/255f);
    }

    public void ExitSettingButton()
    {
        settingButton.transform.localScale = new Vector3(0.7f, 0.5f, 1.0f);
        settingButton.GetComponent<Image>().color = new Color(224f / 255f, 224f / 255f, 224f / 255f);

    }

    public void ExitQuitButton()
    {
        quitButton.transform.localScale = new Vector3(0.7f, 0.5f, 1.0f);
        quitButton.GetComponent<Image>().color = new Color(224f / 255f, 224f / 255f, 224f / 255f);

    }

    public void ClickQuitButton()
    {
        Application.Quit();
    }

}
