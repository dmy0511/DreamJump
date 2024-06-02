using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yaong : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(PlayAudioAfterDelay(2.5f));
    }

    private IEnumerator PlayAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        audioSource.Play();
    }
}
