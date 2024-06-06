using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FbLineGenerator : MonoBehaviour
{
    // 발판 라인 프리팹
    public GameObject FbLine;

    GameObject player;

    // 발판 라인을 생성할 높이 간격
    float createHeight = 10.0f;

    // 최근 생성된 발판 라인의 높이
    float recentHeight = -2.5f;

    // 발판 생성 확률 (3개, 2개, 1개)
    [SerializeField]
    float[] probability = { 0.6f, 0.35f, 0.05f };   // <- 발판 확률 조정은 여기서

    void Start()
    {
        this.player = GameObject.Find("cat");

        // 첫 번째 발판 라인 생성
        GameObject firstFbWave = Instantiate(FbLine) as GameObject;
        firstFbWave.transform.position = new Vector3(0.0f, recentHeight, 0.0f);
        firstFbWave.GetComponent<FbLineCtrl>().SetHideFootboards(0);
    }

    void Update()
    {
        Vector3 playerPos = this.player.transform.position;

        // 플레이어 위치에서 createHeight 만큼 위로 올라가면 새로운 발판 라인 생성
        if (recentHeight < playerPos.y + createHeight)
        {
            SpawnFbWave(recentHeight);
            recentHeight += 2.5f;
        }
    }

    void SpawnFbWave(float a_Height)
    {
        // 발판 라인의 레벨 계산 (높이에 따라 달라짐)
        int a_Level = (int)(a_Height / 15.0f);

        // 랜덤 값에 따라 숨길 발판 개수 결정
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

        // 새로운 발판 라인 생성
        GameObject go = Instantiate(FbLine) as GameObject;
        go.transform.position = new Vector3(0.0f, a_Height, 0.0f);
        go.GetComponent<FbLineCtrl>().SetHideFootboards(a_HideCount);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // "DeadlineRoot" 또는 "cat" 이름의 게임 오브젝트와 충돌하면 비활성화
        if (other.gameObject.name == "DeadlineRoot" || other.gameObject.name == "cat")
        {
            other.gameObject.SetActive(false);
        }
    }
}
