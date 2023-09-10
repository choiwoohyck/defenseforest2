using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnergyComponent : MonoBehaviour
{
    // Start is called before the first frame update

    public int smallEnergyCnt;
    public float maxTimer = 0.5f;

    public float timer = 0;

    public GameObject LargeEnergy;

    public bool startGathering = false;

    public bool timerStart = false;
    float waitTimer = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<Element>().isBuilding || !startGathering || !GameManager.instance.isGameTurn)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }

        else if (startGathering && GetComponent<Element>().isBuilding && GameManager.instance.isGameTurn)
            transform.GetChild(0).gameObject.SetActive(true);

        if (!GameManager.instance.isGameTurn) return;

        if (timerStart)
            waitTimer += Time.deltaTime;

        if (waitTimer > 1f)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            timerStart = false;
            waitTimer = 0;
            startGathering = true;
        }

        if (startGathering)
            timer += Time.deltaTime;

   
        if (timer >= 5)
        {
            GameObject largeEnergy = Instantiate(LargeEnergy,transform.position,transform.rotation) as GameObject;
            largeEnergy.GetComponent<LargeEnergy>().owner = gameObject;
            transform.GetChild(0).gameObject.SetActive(false);
            startGathering = false;
            timer = 0;
        }

        
    }
}
