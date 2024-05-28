using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    SpriteRenderer sprite;
    public float moveSpeed = 0.1f;
    public float minSize = 0.1f;
    public float maxSize = 0.3f;
    public float sizeSpeed = 1.0f;
    public Color[] colors;
    public float colorSpeed = 5;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        float size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector2(size, size);

        sprite.color = colors[Random.Range(0, colors.Length)];
    }

    void Update()
    {
        transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, Time.deltaTime * sizeSpeed);

        Color color = sprite.color;
        color.a = Mathf.Lerp(sprite.color.a, 0, Time.deltaTime * colorSpeed);
        sprite.color = color;

        if (sprite.color.a <= 0.01f)
            Destroy(gameObject);
    }
}
