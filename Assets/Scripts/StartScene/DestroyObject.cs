using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToDestroy;

    [SerializeField] private Button destroyButton;

    [SerializeField] private float fadeOutTime = 0.25f;

    public void DestroyUI()
    {
        foreach (GameObject obj in objectsToDestroy)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                StartCoroutine(FadeOutAndDestroy(obj, renderer));
            }
            else
            {
                Destroy(obj);
            }
        }
    }

    private IEnumerator FadeOutAndDestroy(GameObject obj, Renderer renderer)
    {
        Color originalColor = renderer.material.color;

        float elapsedTime = 0.0f;
        while (elapsedTime < fadeOutTime)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeOutTime);
            
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            
            renderer.material.color = newColor;

            yield return null;
            
            elapsedTime += Time.deltaTime;
        }

        Destroy(obj);
    }
}
