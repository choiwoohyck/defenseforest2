using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Player;
    public bool isBoss = false;
    public float lerpSpeed = 0.5f;
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

        if (isBoss)
        {
            xPos = -0.25f;
            yPos = 9.7f;
        }

        Vector3 camPos = new Vector3(xPos, yPos, transform.position.z);
        transform.position = camPos;
        transform.position = Vector3.Lerp(transform.position, camPos, Time.deltaTime * lerpSpeed);
    }

    private void ResolutionFix()
    {
        // 가로 세로 비율
        float targetWidthAspect = 16.0f;
        float targetHeightAspect = 9.0f;

        Camera.main.aspect = targetWidthAspect / targetHeightAspect;
        Application.targetFrameRate = 60;

    }

}
