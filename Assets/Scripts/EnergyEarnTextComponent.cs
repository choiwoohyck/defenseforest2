using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnergyEarnTextComponent : MonoBehaviour
{
    // Start is called before the first frame update
    TextMesh tm;

    void Start()
    {
        tm = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * 2f * Time.deltaTime);

        tm.color = new Color(tm.color.r, tm.color.g, tm.color.b, tm.color.a - Time.deltaTime * 3f);
        if (tm.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
