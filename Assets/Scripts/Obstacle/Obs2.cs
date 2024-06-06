using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obs2 : MonoBehaviour
{
    public GameObject obsPrefab1;
    public GameObject obsPrefab2;
    public float spawn1 = 2.0f;  //厘局拱 积己 林扁
    public float spawn2 = 2.0f;  //厘局拱 积己 林扁
    float delta = 0.0f;

    float[] spawnPositions = { -2.0f, -0.67f, 0.67f, 2.0f };

    void Start()
    {

    }

    void Update()
    {
        delta += Time.deltaTime;
        if (delta > spawn1)
        {
            delta = 0.0f;
            GameObject go = Instantiate(obsPrefab1) as GameObject;

            float dropPosX = spawnPositions[Random.Range(0, spawnPositions.Length)];
            go.GetComponent<ObsController>().InitObs(dropPosX);
        }

        if (delta > spawn2)
        {
            delta = 0.0f;
            GameObject go = Instantiate(obsPrefab2) as GameObject;

            float dropPosX = spawnPositions[Random.Range(0, spawnPositions.Length)];
            go.GetComponent<ObsController>().InitObs(dropPosX);
        }
    }
}
