using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBoundaryComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
