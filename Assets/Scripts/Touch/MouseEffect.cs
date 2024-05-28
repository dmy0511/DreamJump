using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEffect : MonoBehaviour
{
    public GameObject touch;
    float spawnsTime;
    public float defaultTime = 0.05f;

    void Update()
    {
        if (Input.GetMouseButton(0) && spawnsTime >= defaultTime)
        {
            EffectCreat();
            spawnsTime = 0;
        }
        spawnsTime += Time.deltaTime;
    }

    void EffectCreat()
    {
        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPosition.z = 0;
        Instantiate(touch, mPosition, Quaternion.identity);
    }
}
