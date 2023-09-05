using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnitManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static AllyUnitManager instance;
    public List<GameObject> allyUnits = new List<GameObject>();
    public List<GameObject> noBuildElements = new List<GameObject>();

    public bool isBuild = true;
    public bool alreadyClick = false;

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
}
