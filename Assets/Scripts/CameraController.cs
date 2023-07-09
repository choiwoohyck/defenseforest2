using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Player;
    void Start()
    {
        ResolutionFix();
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = Player.transform.position.x;
        float yPos = Player.transform.position.y;

        xPos = Mathf.Clamp(xPos, -4.1f, 4.1f);
        yPos = Mathf.Clamp(yPos, 2, 10);

        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }

    private void ResolutionFix()
    {
        // 가로 세로 비율
        float targetWidthAspect = 16.0f;
        float targetHeightAspect = 9.0f;

        Camera.main.aspect = targetWidthAspect / targetHeightAspect;

    }

}
