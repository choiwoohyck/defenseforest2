using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StageManager : MonoBehaviour
{
    [HideInInspector]
    public static StageManager instance;

    public GameObject GlobalLight;
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
        
    }

    public void ChangeNight()
    {

        GlobalLight.GetComponent<Light2D>().color = new Color(10/255f, 10/255f, 10/255f);
    }
}
