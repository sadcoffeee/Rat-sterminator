using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class DropShadow : MonoBehaviour
{
    public Vector2 ShadowOffset;
    public Material ShadowMaterial;
    public GameObject shadowGameobject;

    SpriteRenderer spriteRenderer;
    SpriteRenderer shadowSpriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        Vector3 spawnPos = new Vector3(transform.position.x + ShadowOffset.x, transform.position.y + ShadowOffset.y, transform.position.z);

        shadowGameobject = Instantiate(shadowGameobject, spawnPos, Quaternion.identity, transform);

        shadowSpriteRenderer = shadowGameobject.GetComponent<SpriteRenderer>();

        shadowSpriteRenderer.sprite = spriteRenderer.sprite;

        shadowSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
    }

    void LateUpdate()
    {
        if (spriteRenderer.flipX) { shadowSpriteRenderer.flipX = true; }
        if (!spriteRenderer.flipX) { shadowSpriteRenderer.flipX = false; }
    }
}
