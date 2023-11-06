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
        if (UIManager.instance.SettingUI.activeSelf) return;

        if (GameManager.instance.energy < 100) return;

        if (!AllyUnitManager.instance.alreadyClick)
        {
            GameManager.instance.energy -= 100;
            ClearNoBuildElement();
            GameObject fire = Instantiate(ElementPrefab[(int)ElementType.FIRE], transform.position, Quaternion.identity) as GameObject;
            fire.GetComponent<Element>().price = 100;
            AllyUnitManager.instance.noBuildElements.Add(fire);
            AllyUnitManager.instance.alreadyClick = true;
        }
    }

    public void SpawnIceElement()
    {
        if (UIManager.instance.SettingUI.activeSelf) return;

        if (GameManager.instance.energy < 150) return;

        if (!AllyUnitManager.instance.alreadyClick)
        {
            GameManager.instance.energy -= 150;
            ClearNoBuildElement();
            GameObject ice = Instantiate(ElementPrefab[(int)ElementType.ICE], transform.position, Quaternion.identity) as GameObject;
            ice.GetComponent<Element>().price = 150;
            AllyUnitManager.instance.noBuildElements.Add(ice);
            AllyUnitManager.instance.alreadyClick = true;
        }
    }

    public void SpawnLeafElement()
    {
        if (UIManager.instance.SettingUI.activeSelf) return;

        if (GameManager.instance.energy < 200) return;

        if (!AllyUnitManager.instance.alreadyClick)
        {
            GameManager.instance.energy -= 200;
            ClearNoBuildElement();
            GameObject leaf = Instantiate(ElementPrefab[(int)ElementType.LEAF], transform.position, Quaternion.identity) as GameObject;
            leaf.GetComponent<Element>().price = 200;
            AllyUnitManager.instance.noBuildElements.Add(leaf);
            AllyUnitManager.instance.alreadyClick = true;
        }
    }

    public void SpawnStoneElement()
    {
        if (UIManager.instance.SettingUI.activeSelf) return;

        if (GameManager.instance.energy < 200) return;

        if (!AllyUnitManager.instance.alreadyClick)
        {
            GameManager.instance.energy -= 200;
            ClearNoBuildElement();
            GameObject stone = Instantiate(ElementPrefab[(int)ElementType.STONE], transform.position, Quaternion.identity) as GameObject;
            stone.GetComponent<Element>().price = 200;
            AllyUnitManager.instance.noBuildElements.Add(stone);
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
