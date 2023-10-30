using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumManagerSpace;
public class EffectManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static EffectManager instance;


    EffectType type;

    public GameObject[] EffectPrefab;

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

    public void CreateEffect(EffectType type, Vector3 effectPos, Quaternion rotation, Vector3? scale = null)
    {

        GameObject effect = Instantiate(EffectPrefab[(int)type]) as GameObject;
        effect.transform.position = effectPos;
        effect.transform.rotation = rotation;

        if (scale != null)
            effect.transform.localScale = (Vector3)scale;
    }
}
