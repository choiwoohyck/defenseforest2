using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeEnergy : MonoBehaviour
{
    // Start is called before the first frame update

    float originY;
    public float moveSpeed = 3f;

    bool isDown = false;

    public GameObject owner;
    public GameObject textPrefab;
    

    void Start()
    {
        originY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < originY + 1.2f)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
        else
            isDown = true;

        if (isDown)
        {
            if (transform.position.y <= originY)
            {
                GameManager.instance.energy += 15;
                EffectManager.instance.CreateEffect(EnumManagerSpace.EffectType.GATHERINGENERGY, transform.position, transform.rotation);

                owner.GetComponent<CreateEnergyComponent>().timerStart = true;

                GameObject text = Instantiate(textPrefab) as GameObject;
                text.transform.position = transform.position + new Vector3(0.5f, 0);

                Destroy(gameObject);
            }

            transform.Translate(Vector2.down * moveSpeed*2f * Time.deltaTime);
        }
        
    }
}
