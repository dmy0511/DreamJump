using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance;

    GameObject player;

    public Text Height_Text;
    public Text CurScore_Text;
    public Text BestScore_Text;

    public static float m_CurScore = 0.0f;
    public static float m_BestScore = 0.0f;
    public static float totalHeight = 0.0f;
    public static float currentSceneHeight = 0.0f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (Height_Text != null)
            {
                DontDestroyOnLoad(Height_Text.transform.parent.gameObject);
            }

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
        if (scene.name == "FirstScene")
        {
            totalHeight = 0;
            currentSceneHeight = 0;
            m_CurScore = 0.0f;
            PlayerPrefs.SetFloat("AccumulatedHeight", 0f);
            if (Height_Text != null)
            {
                Height_Text.text = "0m";
            }
            if (CurScore_Text != null)
                CurScore_Text.text = "점수 : " + FormatScoreString(m_CurScore);
        }
        else if (scene.name != "StartScene" && scene.name != "OverScene")
        {
            float prevTotalHeight = PlayerPrefs.GetFloat("AccumulatedHeight", 0f);
            prevTotalHeight += currentSceneHeight;
            PlayerPrefs.SetFloat("AccumulatedHeight", prevTotalHeight);
            currentSceneHeight = 0;

            totalHeight = prevTotalHeight;
            if (Height_Text != null)
            {
                Height_Text.text = Mathf.Floor(totalHeight).ToString() + "m";
            }
        }
    }

    void Start()
    {
        Load();
        player = GameObject.Find("cat");

        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "FirstScene")
        {
            totalHeight = 0;
            currentSceneHeight = 0;
            m_CurScore = 0.0f;
            PlayerPrefs.SetFloat("AccumulatedHeight", 0f);
        }
        else if (sceneName != "StartScene" && sceneName != "OverScene")
        {
            totalHeight = PlayerPrefs.GetFloat("AccumulatedHeight", 0f);
            currentSceneHeight = 0;
        }

        if (Height_Text != null)
        {
            Height_Text.text = FormatHeightString(totalHeight) + "m";
        }

        if (CurScore_Text != null)
            CurScore_Text.text = "점수 : " + FormatScoreString(m_CurScore);

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
        player = GameObject.Find("cat");

        if (player != null && Height_Text != null)
        {
            float rawPlayerHeight = Mathf.Max(0, player.transform.position.y);
            float amplifiedHeight = rawPlayerHeight * 5;

            currentSceneHeight = amplifiedHeight;
            totalHeight = currentSceneHeight + PlayerPrefs.GetFloat("AccumulatedHeight", 0f);

            Height_Text.text = FormatHeightString(totalHeight) + "m";
        }

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

        if (Input.GetKeyDown(KeyCode.K)) //치트키
        {
            m_BestScore = 0.0f;
            Save();
            if (BestScore_Text != null)
                BestScore_Text.text = "최고점 : " + FormatScoreString(m_BestScore);
        }
    }

    string FormatScoreString(float score)
    {
        string scoreString = Mathf.Floor(score).ToString();

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

    string FormatHeightString(float height)
    {
        string heightString = Mathf.Floor(height).ToString();

        if (height >= 1000)
        {
            int length = heightString.Length;
            int commaCount = (length - 1) / 3;

            for (int i = 1; i <= commaCount; i++)
            {
                heightString = heightString.Insert(length - i * 3, ",");
            }
        }

        return heightString;
    }

    public static void Save()
    {
        PlayerPrefs.SetFloat("HighScore", m_BestScore);
        PlayerPrefs.SetFloat("AccumulatedHeight", totalHeight);
    }

    public static void Load()
    {
        m_BestScore = PlayerPrefs.GetFloat("HighScore", 0f);
        totalHeight = PlayerPrefs.GetFloat("AccumulatedHeight", 0f);
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
