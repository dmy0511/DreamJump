using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlineCtrl : MonoBehaviour
{
    GameObject player;

    // 데드라인의 이동 속도
    public float speed = 1.0f;

    // 플레이어와 데드라인 사이의 거리 간격
    float distanceItv = 8.0f;

    void Start()
    {
        player = GameObject.Find("cat");
    }

    void Update()
    {
        // 플레이어의 Y값에서 플레이어와 데드라인 사이의 거리 간격만큼 아래에 있어야 하는 데드라인의 Y값 계산
        float a_FollowHeight = player.transform.position.y - distanceItv;

        // 현재 데드라인의 Y값이 a_FollowHeight 보다 작으면 a_FollowHeight 위치로 이동
        if (transform.position.y < a_FollowHeight)
            transform.position = new Vector3(0.0f, a_FollowHeight, 0.0f);

        // 데드라인을 speed 속도로 아래로 이동
        transform.Translate(new Vector3(0.0f, speed * Time.deltaTime, 0.0f));
    }
}
