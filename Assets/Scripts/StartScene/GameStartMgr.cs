using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartMgr : MonoBehaviour
{
    public Button startBtn;
    public GameObject explain;

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

    public void Explain_Exit()
    {
        explain.SetActive(false);
    }
}
