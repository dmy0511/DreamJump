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

    public static float m_CurScore = 0.0f;    // 현재 최고 높이
    public static float m_BestScore = 0.0f;    // 최고 기록 높이

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
        if (scene.name == "StartScene" || scene.name == "OverScene")
        {
            SetActiveConnectedObjects(false);
            gameObject.SetActive(false);
        }
        else
        {
            SetActiveConnectedObjects(true);
            gameObject.SetActive(true);
        }
    }

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "StartScene" || sceneName == "OverScene")
        {
            SetActiveConnectedObjects(false);
            gameObject.SetActive(false);
        }
        else
        {
            SetActiveConnectedObjects(true);
            gameObject.SetActive(true);
        }

        Load();

        player = GameObject.Find("cat");
        m_CurScore = 0.0f;
    }

    void Update()
    {
        if (m_BestScore < m_CurScore)
        {
            m_BestScore = m_CurScore;
            Save();
        }

        if (CurScore_Text != null)
            CurScore_Text.text = "점수 : " + m_CurScore.ToString();

        if (BestScore_Text != null)
            BestScore_Text.text = "최고점 : " + m_BestScore.ToString();
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
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (CurScore_Text != null)
        {
            if (isActive && (currentSceneName == "StartScene" || currentSceneName == "OverScene" || currentSceneName == "FirstScene"))
            {
                CurScore_Text.transform.parent.gameObject.SetActive(true);
                m_CurScore = 0.0f;
                CurScore_Text.text = "점수 : " + m_CurScore.ToString();
            }
            else
            {
                CurScore_Text.transform.parent.gameObject.SetActive(isActive);
            }
        }

        if (BestScore_Text != null)
        {
            BestScore_Text.transform.parent.gameObject.SetActive(isActive);
        }
    }
}
