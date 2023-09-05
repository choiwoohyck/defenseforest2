using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddictedComponent : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isAddicted = false;
    bool isShake = false;

    public float poisonDamage = 3;
    float addictTimer = 0;

    Vector3 originPos;
    float shakeDecay = 5f;
    public float shakeIntensity = 1;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAddicted)
        {
            addictTimer += Time.deltaTime;
            if (addictTimer>=0.5f)
            {
                gameObject.GetComponent<Enemy>().hp -= poisonDamage;
                gameObject.GetComponent<HitObject>().ChangeAddictColor();
                isShake = true;
                addictTimer = 0;
                originPos = transform.position;
                shakeIntensity = 1;
            }

            if (isShake)
            {
                if (shakeIntensity > 0)
                {
                    transform.position = originPos + new Vector3(Random.Range(0f, 0.2f), 0) * shakeIntensity;
                    shakeIntensity -= shakeDecay*Time.deltaTime;
                }
                else
                    isShake = false;
            }
        }
    }

    
}
