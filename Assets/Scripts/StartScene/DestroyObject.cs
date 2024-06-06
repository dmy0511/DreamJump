using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyObject : MonoBehaviour
{
    // 제거할 게임 오브젝트 목록
    [SerializeField] private List<GameObject> objectsToDestroy;

    // 제거 버튼 UI 요소
    [SerializeField] private Button destroyButton;

    // 페이드 아웃 시간 (초)
    [SerializeField] private float fadeOutTime = 0.25f;

    // UI 요소 제거 함수
    public void DestroyUI()
    {
        // objectsToDestroy 리스트의 각 게임 오브젝트에 대해
        foreach (GameObject obj in objectsToDestroy)
        {
            // 게임 오브젝트에 Renderer 컴포넌트가 있는지 확인
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Renderer 컴포넌트가 있으면 FadeOutAndDestroy 코루틴 실행
                StartCoroutine(FadeOutAndDestroy(obj, renderer));
            }
            else
            {
                // Renderer 컴포넌트가 없으면 바로 제거
                Destroy(obj);
            }
        }
    }

    // 게임 오브젝트를 페이드 아웃하며 제거하는 코루틴
    private IEnumerator FadeOutAndDestroy(GameObject obj, Renderer renderer)
    {
        // 원본 색상 저장
        Color originalColor = renderer.material.color;
        float elapsedTime = 0.0f;

        // fadeOutTime 동안 반복
        while (elapsedTime < fadeOutTime)
        {
            // 현재 시간에 따른 alpha 값 계산
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeOutTime);

            // 새로운 색상 설정 (alpha 값 적용)
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            renderer.material.color = newColor;

            // 다음 프레임까지 대기
            yield return null;

            // 경과 시간 증가
            elapsedTime += Time.deltaTime;
        }

        // fadeOutTime이 지나면 게임 오브젝트 제거
        Destroy(obj);
    }
}
