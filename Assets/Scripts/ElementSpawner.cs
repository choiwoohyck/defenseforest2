using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;

public class ElementSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] ElementPrefab;

    public GameObject BuildBorder;
    public static ElementSpawner instance;

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnFireElement();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnIceElement();
        }
    }
    public void SpawnFireElement()
    {
        if (!AllyUnitManager.instance.alreadyClick)
        {
            ClearNoBuildElement();
            GameObject fire = Instantiate(ElementPrefab[(int)ElementType.FIRE], transform.position, Quaternion.identity) as GameObject;
            AllyUnitManager.instance.noBuildElements.Add(fire);
            AllyUnitManager.instance.alreadyClick = true;
        }
    }

    public void SpawnIceElement()
    {
        if (!AllyUnitManager.instance.alreadyClick)
        {
            ClearNoBuildElement();
            GameObject ice = Instantiate(ElementPrefab[(int)ElementType.ICE], transform.position, Quaternion.identity) as GameObject;
            AllyUnitManager.instance.noBuildElements.Add(ice);
            AllyUnitManager.instance.alreadyClick = true;
        }
    }


    void ClearNoBuildElement()
    {
        foreach (GameObject e in AllyUnitManager.instance.noBuildElements)
        {
            AllyUnitManager.instance.noBuildElements.Remove(e);
            Destroy(e.gameObject);
            break;
        }
    }
    
}
