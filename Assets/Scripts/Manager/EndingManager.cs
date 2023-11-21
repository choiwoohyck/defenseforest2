using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doublsb.Dialog;
using UnityEngine.UI;
#if UNITY_EDITOR
using static UnityEditor.PlayerSettings;
#endif
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [HideInInspector]
    public static EndingManager instance;
    public DialogManager DialogManager;

    public bool isEnding = false;
    public bool firstSetting = false;
    public GameObject magicStone;
    public GameObject Printer;
    public GameObject Character;
    public GameObject treePrefab;
    public GameObject Player;
    public GameObject PauseButton;
    public GameObject TheEndText;
    public GameObject MadeByText;

    public Camera mainCamera;

    public Image deadBackGround;


    float treeSpawnTimer = 0;
    float textSpawnTimer = 0f;
    int treeCnt = 0;
    
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
        if (isEnding && !firstSetting)
        {
            DialogManager.isClickSkipButton = false;
            magicStone.transform.GetChild(0).gameObject.SetActive(false);
            magicStone.transform.GetChild(2).gameObject.SetActive(true);
            Printer.SetActive(true);
            Character.SetActive(true);
            PauseButton.SetActive(false);

            Player.GetComponent<MoveController>().isStop = true;

            DialogManager.Show(new DialogData("덕분에 힘을 되찾아서 이제 숲을 재건할 수 있겠어 정말 고마워!", "MagicStone"));
            firstSetting = true;

            Player.transform.position = new Vector3(0f, 4.5f, -10f);
        }

        if (isEnding)
        {
            treeSpawnTimer += Time.deltaTime;
            AudioManager.instance.isFadeOut = true;
            if (mainCamera.orthographicSize <= 7.18)
                mainCamera.orthographicSize += Time.deltaTime * 2f;

            if (treeSpawnTimer >= 0.5f && treeCnt < 10)
            {
                SpawnTree();
                treeSpawnTimer = 0f;
                treeCnt++;

             
            }


            if (treeCnt >= 3)
            {
                Character.SetActive(false);
                Printer.SetActive(false);
            }

            if (treeCnt >= 9)
            {
                deadBackGround.gameObject.SetActive(true);
                deadBackGround.fillAmount += Time.deltaTime;
                
                if (deadBackGround.fillAmount >= 1)
                {
                    textSpawnTimer += Time.deltaTime;
                    if (textSpawnTimer >= 1.0f && !TheEndText.activeSelf)
                    {
                        TheEndText.SetActive(true);
                        textSpawnTimer = 0;
                    }

                    if (textSpawnTimer >= 1.0f && TheEndText.activeSelf && !MadeByText.activeSelf)
                    {
                        MadeByText.SetActive(true);
                        textSpawnTimer = 0;

                    }

                    if (TheEndText.activeSelf && MadeByText.activeSelf && textSpawnTimer >= 2.0f)
                    {
                        SceneManager.LoadScene("MainScene");
                    }
                }
            }
        }
    }
    void SpawnTree()
    {
        int randomX = Random.Range(2, 26);
        int randomY = Random.Range(5, 15);
        while (!TileMapManager.instance.tiles[randomX, randomY].isTree)
        {
            randomX = Random.Range(2, 26);
            randomY = Random.Range(5, 15);
            Vector3 spawnPos = new Vector3(randomX - 12.5f, 14.5f - randomY, 0);
            Instantiate(treePrefab, spawnPos, Quaternion.identity);
            TileMapManager.instance.tiles[randomX, randomY].isTree = true;
        }
    }
   
}
