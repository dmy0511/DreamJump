using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartMgr : MonoBehaviour
{
    public Button startBtn;

    void Start()
    {
        if (startBtn != null)
        {
            startBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("FirstScene");
            });
        }
    }

    void Update()
    {
        
    }
}
