using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbLineGenerator : MonoBehaviour
{
    public GameObject FbLine;
    GameObject player;

    float createHeight = 10.0f;
    //player로부터 머리위로 10.0m 위까지만 발판을 생성하겠다는 의미
    
    float recentHeight = -2.5f; //마지막 생성된 발판의 높이

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.Find("cat");

        for (int i = 0; i < 4; i++)
        {
            SpawnFbWave(player.transform.position.y - 2.5f - (i * 2.5f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = this.player.transform.position;

        //일정 높이에 발판 생성
        if (recentHeight < playerPos.y + createHeight)
        {
            SpawnFbWave(recentHeight);
            recentHeight += 2.5f;
        }
    }

    void SpawnFbWave(float a_Height)
    {
        int a_Level = (int)(a_Height / 15.0f);

        // 발판 갯수별 확률 설정
        float[] probability = { 0.1f, 0.3f, 0.6f };

        // 랜덤하게 발판 갯수 선택
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
