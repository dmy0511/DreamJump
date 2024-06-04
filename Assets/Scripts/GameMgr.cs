using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance;

    GameObject player;

    public Text CurScore_Text;
    public Text BestScore_Text;

    public static float m_CurScore = 0.0f;
    public static float m_BestScore = 0.0f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (CurScore_Text != null)
            {
                DontDestroyOnLoad(CurScore_Text.transform.parent.gameObject);
            }
            if (BestScore_Text != null)
            {
                DontDestroyOnLoad(BestScore_Text.transform.parent.gameObject);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateGameObjectActiveState();
    }

    void Start()
    {
        Load();

        player = GameObject.Find("cat");

        m_CurScore = 0.0f;

        UpdateGameObjectActiveState();
    }

    void UpdateGameObjectActiveState()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        bool isActive = !(sceneName == "StartScene" || sceneName == "OverScene");

        gameObject.SetActive(isActive);

        SetActiveConnectedObjects(isActive);
    }

    void Update()
    {
        if (m_BestScore < m_CurScore && SceneManager.GetActiveScene().name == "OverScene")
        {
            m_BestScore = m_CurScore;
            Save();
        }

        m_CurScore = Mathf.Max(0, m_CurScore);

        if (CurScore_Text != null)
            CurScore_Text.text = "점수 : " + FormatScoreString(m_CurScore);

        if (BestScore_Text != null)
            BestScore_Text.text = "최고점 : " + FormatScoreString(m_BestScore);

        if (Input.GetKeyDown(KeyCode.K))    //치트키
        {
            m_BestScore = 0.0f;
            Save();
            if (BestScore_Text != null)
                BestScore_Text.text = "최고점 : " + FormatScoreString(m_BestScore);
        }
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

    public static void Save()
    {
        PlayerPrefs.SetFloat("HighScore", m_BestScore);
    }

    public static void Load()
    {
        m_BestScore = PlayerPrefs.GetFloat("HighScore", 0.0f);
    }

    void SetActiveConnectedObjects(bool isActive)
    {
        if (CurScore_Text != null)
        {
            CurScore_Text.transform.parent.gameObject.SetActive(isActive);
        }

        if (BestScore_Text != null)
        {
            BestScore_Text.transform.parent.gameObject.SetActive(isActive);
        }
    }
}
