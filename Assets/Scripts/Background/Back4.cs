using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back4 : MonoBehaviour
{
    GameObject player;
    float startY = 12.0f;
    float scroll = 0.3f;

    void Start()
    {
        player = GameObject.Find("cat");
    }

    void Update()
    {
        float scrollPos = startY - player.transform.position.y * scroll;
        if (scrollPos > 12.0f)
            scrollPos = 12.0f;
        else if (scrollPos < -41.85f)
            scrollPos = -41.85f;

        transform.position = new Vector3(0.0f,
                                player.transform.position.y + scrollPos, 0.0f);
    }
}
