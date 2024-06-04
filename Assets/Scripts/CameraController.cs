using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    GameObject player;

    public GameObject firstline;
    private FbLineCtrl fbLineCtrl;
    private float yThreshold = 60f;
    private string targetSceneName = "FirstScene";

    void Start()
    {
        this.player = GameObject.Find("cat");
    }

    void Update()
    {
        Vector3 playerPos = this.player.transform.position;
        transform.position = new Vector3(
            transform.position.x, playerPos.y, transform.position.z);

        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == targetSceneName)
        {
            fbLineCtrl = firstline.GetComponent<FbLineCtrl>();

            float cameraY = transform.position.y;
            if (cameraY >= yThreshold)
            {
                fbLineCtrl.SetItemProbabilitiesToZero();
            }
        }
        else if (currentSceneName != targetSceneName)
        {
            return;
        }
    }
}
