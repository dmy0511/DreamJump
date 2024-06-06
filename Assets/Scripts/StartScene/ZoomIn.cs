using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomIn : MonoBehaviour
{
    private float zoom;

    // 최소 줌 레벨
    private float minZoom = 2.5f;

    // 카메라 줌 움직임의 속도
    private float velocity = 0f;

    // 카메라 줌 움직임의 부드러움 정도
    private float smoothTime = 0.5f;

    [SerializeField] private Camera cam;

    [SerializeField] private Button zoomInButton;

    [SerializeField] private GameObject startButton;

    [SerializeField] private GameObject title;

    private void Start()
    {
        // 시작 버튼과 타이틀 게임 오브젝트를 비활성화
        startButton.SetActive(false);
        title.SetActive(false);
    }

    public void ZoomInButtonClicked()
    {
        AudioSource CageDoor = GetComponent<AudioSource>();

        // 오디오 재생
        CageDoor.Play();

        // ZoomInCoroutine 코루틴 시작
        StartCoroutine(ZoomInCoroutine());
    }

    private IEnumerator ZoomInCoroutine()
    {
        // 현재 줌 레벨과 목표 줌 레벨
        float currentZoom = cam.orthographicSize;
        float targetZoom = minZoom;

        // 목표 줌 레벨에 도달할 때까지 반복
        while (Mathf.Abs(cam.orthographicSize - targetZoom) > 0.01f)
        {
            // 카메라 줌 레벨을 부드럽게 변경
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref velocity, smoothTime);

            yield return null;
        }

        // 줌인이 완료되면 타이틀과 시작 버튼 활성화
        title.SetActive(true);
        startButton.SetActive(true);
    }
}
