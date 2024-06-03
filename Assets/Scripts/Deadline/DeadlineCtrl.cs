using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlineCtrl : MonoBehaviour
{
    GameObject player;
    public float speed = 1.0f;  //데드라인 속도
    float distanceItv = 8.0f;

    void Start()
    {
        player = GameObject.Find("cat");
    }

    void Update()
    {
        float a_FollowHeight = player.transform.position.y - distanceItv;
        if (transform.position.y < a_FollowHeight)
            transform.position = new Vector3(0.0f, a_FollowHeight, 0.0f);

        transform.Translate(new Vector3(0.0f, speed * Time.deltaTime, 0.0f));
    }
}
