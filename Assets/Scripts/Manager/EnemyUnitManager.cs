using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static EnemyUnitManager instance;
    public List<GameObject> enemyUnits = new List<GameObject>();

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
