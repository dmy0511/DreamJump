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
            bestScoreText.text = "최고점 : " + FormatScoreString(GameMgr.m_BestScore);

        if (currentScoreText != null)
            currentScoreText.text = "점수 : " + FormatScoreString(GameMgr.m_CurScore);

        if (rstBtn != null)
        {
            rstBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("FirstScene");
            });
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("FirstScene");
        }

        if (Input.GetKeyDown(KeyCode.K)) //치트키
        {
            PlayerPrefs.DeleteAll();

            GameMgr.Load();

            if (bestScoreText != null)
                bestScoreText.text = "최고점 : " + FormatScoreString(GameMgr.m_BestScore);

            if (currentScoreText != null)
                currentScoreText.text = "점수 : " + FormatScoreString(GameMgr.m_CurScore);
        }

    }

    public void GameExit()
    {
        Application.Quit();
    }

    string FormatScoreString(float score)
    {
        string scoreString = score.ToString();

        if (score >= 1000)
        {
            int length = scoreString.Length;
            int commaCount = (length - 1) / 3;

            for (int i = 1; i <= commaCount; i++)
            {
                scoreString = scoreString.Insert(length - i * 3, ",");
            }
        }

        return scoreString;
    }
}
