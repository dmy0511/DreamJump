using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMgr : MonoBehaviour
{
    public Text bestScoreText;
    public Text currentScoreText;
    public Button rstBtn;

    void Start()
    {
        if (GameMgr.m_BestScore < GameMgr.m_CurScore)
        {
            GameMgr.m_BestScore = GameMgr.m_CurScore;
            GameMgr.Save();
        }

        if (bestScoreText != null)
            bestScoreText.text = "최고점 : " + GameMgr.m_BestScore.ToString();

        if (currentScoreText != null)
            currentScoreText.text = "점수 : " + GameMgr.m_CurScore.ToString();

        if(rstBtn != null)
        {
            rstBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("FirstScene");
            });
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("FirstScene");
        }

        if(Input.GetKeyDown(KeyCode.K)) //치트키
        {
            PlayerPrefs.DeleteAll();    //저장 값 모두 초기화 하기

            GameMgr.Load();

            if (bestScoreText != null)
                bestScoreText.text = "최고점 : " + GameMgr.m_BestScore.ToString();

            if (currentScoreText != null)
                currentScoreText.text = "점수 : " + GameMgr.m_CurScore.ToString();
        }

    }

    public void GameExit()
    {
        Application.Quit();
    }
}
