using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundMgr : MonoBehaviour
{
    [SerializeField] private Button optionBtn;
    [SerializeField] private GameObject option;
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private void Start()
    {
        option.SetActive(false);

        SetMusicVolume();
        SetSoundVolume();

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        float volume = musicSlider.value;

        if (musicSlider.value <= 0)
        {
            myMixer.SetFloat("music", Mathf.Log10(volume) * 0);
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
    }

    public void SetSoundVolume()
    {
        float volume = soundSlider.value;
        myMixer.SetFloat("sound", Mathf.Log10(volume) * 100);
    }

    public void Option()
    {
        option.SetActive(true);
    }

    public void Close()
    {
        option.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "OverScene")
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
