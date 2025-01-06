using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkZone : MonoBehaviour
{
    [SerializeField]
    private Color darkColor = new Color(0.5f, 0.5f, 0.5f, 1f); // à√Ç≠Ç∑ÇÈêF

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var spriteRenderer = collision.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = darkColor; // êFÇà√Ç≠Ç∑ÇÈ
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var spriteRenderer = collision.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white; // å≥Ç…ñﬂÇ∑
        }
    }
}

