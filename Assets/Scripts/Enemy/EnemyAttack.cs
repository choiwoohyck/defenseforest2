using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
public class EnemyAttack : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject target = null;
    GameObject parent;
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
}
