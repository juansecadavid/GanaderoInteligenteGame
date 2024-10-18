using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpriteChanger : MonoBehaviour
{
    [SerializeField] private Vector2 OnChangedSize;
    private Sprite originalSprite;
    private Sprite[] randomSprites;
    private SpriteRenderer spriteRenderer;
    private Vector2 OriginalSize;

    void Awake()
    {
        OriginalSize = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    public void Initialize(Sprite[] sprites)
    {
        randomSprites = sprites;
        InitialLocation(-3.7f,7.2f,-4,3f);
    }

    public void ChangeToRandomSprite()
    {
        if (randomSprites != null && randomSprites.Length > 0)
        {
            spriteRenderer.sprite = randomSprites[Random.Range(0, randomSprites.Length)];
            transform.localScale = OnChangedSize;
            spriteRenderer.sortingOrder = 2;
        }
        else
        {
            Debug.LogWarning("El arreglo de sprites aleatorios está vacío o no ha sido asignado.");
        }
    }

    public void ResetToOriginalSprite()
    {
        spriteRenderer.sprite = originalSprite;
        transform.localScale = OriginalSize;
        spriteRenderer.sortingOrder = 1;
    }

    public void InitialLocation(float xMin, float xMax, float yMin, float yMax)
    {
        transform.position = new Vector2(Random.Range(xMin,xMax), Random.Range(yMin,yMax));
    }
}
