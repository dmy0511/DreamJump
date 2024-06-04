using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsGenerator : MonoBehaviour
{
    public GameObject obsPrefab;
    public float spawn = 2.0f;  //장애물 생성 주기
    float delta = 0.0f;

    float[] spawnPositions = { -2.0f, -0.67f, 0.67f, 2.0f };

    void Start()
    {

    }

    void Update()
    {
        delta += Time.deltaTime;
        if (delta > spawn)
        {
            delta = 0.0f;
            GameObject go = Instantiate(obsPrefab) as GameObject;

            float dropPosX = spawnPositions[Random.Range(0, spawnPositions.Length)];
            go.GetComponent<ObsController>().InitObs(dropPosX);
        }
    }
}
