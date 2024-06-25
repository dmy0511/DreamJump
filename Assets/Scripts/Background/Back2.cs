using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back2 : MonoBehaviour
{
    // 플레이어 게임 오브젝트를 저장할 변수
    GameObject player;

    // 배경의 시작 Y값
    float startY = 12.0f;

    // 플레이어 위치에 따른 배경 스크롤 속도
    float scroll = 0.3f;

    void Start()
    {
        // 씬에서 "cat" 이름의 게임 오브젝트를 찾아 player 변수에 할당
        player = GameObject.Find("cat");
    }

    void Update()
    {
        // 플레이어의 Y값에 따라 배경의 위치를 계산
        float scrollPos = startY - player.transform.position.y * scroll;

        // 배경 위치가 12.0f 보다 크면 12.0f로 제한
        if (scrollPos > 12.0f)
            scrollPos = 12.0f;

        // 배경 위치가 -13.5f 보다 작으면 -13.5f로 제한
        else if (scrollPos < -13.5f)
            scrollPos = -13.5f;

        // 배경의 위치를 플레이어의 Y값과 scrollPos를 더한 값으로 설정
        transform.position = new Vector3(0.0f,
                                player.transform.position.y + scrollPos, 0.0f);
    }
}
