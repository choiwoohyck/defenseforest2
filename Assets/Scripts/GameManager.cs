using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public static GameManager instance;

    public float energy = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        init();
    }

    void init()
    {
        energy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
