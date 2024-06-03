using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbLineGenerator : MonoBehaviour
{
    public GameObject FbLine;
    GameObject player;

    float createHeight = 10.0f;
    
    float recentHeight = -2.5f;

    [SerializeField]    //발판 생성확률 (3개, 2개, 1개)
    float[] probability = { 0.6f, 0.35f, 0.05f };

    void Start()
    {
        this.player = GameObject.Find("cat");

        GameObject firstFbWave = Instantiate(FbLine) as GameObject;
        firstFbWave.transform.position = new Vector3(0.0f, recentHeight, 0.0f);
        firstFbWave.GetComponent<FbLineCtrl>().SetHideFootboards(0);
    }

    void Update()
    {
        Vector3 playerPos = this.player.transform.position;

        if (recentHeight < playerPos.y + createHeight)
        {
            SpawnFbWave(recentHeight);
            recentHeight += 2.5f;
        }
    }

    void SpawnFbWave(float a_Height)
    {
        int a_Level = (int)(a_Height / 15.0f);

        int a_HideCount;
        float rand = Random.value;
        if (rand < probability[0])
        {
            a_HideCount = 1;
        }
        else if (rand < probability[0] + probability[1])
        {
            a_HideCount = 2;
        }
        else
        {
            a_HideCount = 3;
        }

        GameObject go = Instantiate(FbLine) as GameObject;
        go.transform.position = new Vector3(0.0f, a_Height, 0.0f);
        go.GetComponent<FbLineCtrl>().SetHideFootboards(a_HideCount);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "DeadlineRoot" || other.gameObject.name == "cat")
        {
            other.gameObject.SetActive(false);
        }
    }
}
