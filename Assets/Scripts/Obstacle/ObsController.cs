using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObsController : MonoBehaviour
{
    public float speed = 7.0f;  //장애물 속도
    GameObject player;
    public Image warningImg;
    float waitTime = 0.75f; //절대 건들지 마시오.

    void Start()
    {
        if(player == null)
           player = GameObject.Find("cat");
    }

    void Update()
    {
        if(0.0f < waitTime)
        {
            waitTime -= Time.deltaTime;
            WarningDirect();
            return;
        }

        if (warningImg.gameObject.activeSelf == true)
            warningImg.gameObject.SetActive(false);

        transform.Translate(0.0f, -speed * Time.deltaTime, 0.0f);
        if (transform.position.y < player.transform.position.y - 10.0f)
            Destroy(gameObject);
    }

    public void InitObs(float a_PosX)
    {
        player = GameObject.Find("cat");
        transform.position = new Vector3(a_PosX,
                                 player.transform.position.y + 10.0f, 0.0f);

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        warningImg.transform.position = new Vector3(screenPos.x,
                warningImg.transform.position.y, warningImg.transform.position.z);
    }

    float alpha = -6.0f;
    void WarningDirect()
    {
        if (warningImg == null)
            return;

        if (warningImg.color.a >= 1.0f)
            alpha = -6.0f;
        else if (warningImg.color.a <= 0.0f)
            alpha = 6.0f;

        warningImg.color = new Color(1.0f, 1.0f, 1.0f,
                        warningImg.color.a + alpha * Time.deltaTime);
    }
}
