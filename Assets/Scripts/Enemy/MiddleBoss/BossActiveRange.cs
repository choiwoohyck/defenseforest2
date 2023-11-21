using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActiveRange : MonoBehaviour
{
    // Start is called before the first frame update

    public MiddleBoss middleBoss;
    public CameraController camera;
    public GameObject Player;
    public bool active = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!active)
        {
            camera.isBoss = false;
            Player.GetComponent<MoveController>().inMiddleBossStage = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && GameManager.instance.Stage == 3 && active)
        {
            camera.isBoss = true;
            if (!GameManager.instance.middleBossKillFail)
                middleBoss.isActive = true;

            Player.GetComponent<MoveController>().inMiddleBossStage = true;

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameManager.instance.Stage == 3 && active)
        {
            camera.isBoss = true;
            if (!GameManager.instance.middleBossKillFail)
                middleBoss.isActive = true;

            Player.GetComponent<MoveController>().inMiddleBossStage = true;

        }
    }
}
