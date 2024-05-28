using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsGenerator : MonoBehaviour
{
    public GameObject obsPrefab;
    float spawn = 2.0f;
    float delta = 0.0f;

    // 가능한 x 좌표 값 배열
    float[] spawnPositions = { -2.0f, -0.63f, 0.7f, 2.0f };

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if (delta > spawn)
        {
            delta = 0.0f;
            GameObject go = Instantiate(obsPrefab) as GameObject;

            // 배열에서 무작위로 x 값 선택
            float dropPosX = spawnPositions[Random.Range(0, spawnPositions.Length)];
            go.GetComponent<ObsController>().InitObs(dropPosX);
        }
    }
}
