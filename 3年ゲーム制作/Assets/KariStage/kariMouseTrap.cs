using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kariMouseTrap : MonoBehaviour
{
    public float stopDuration = 2f; // 敵を停止させる時間（秒）
    public float colorChangeDuration = 2f; // 色を変える時間（秒）
    public Color changedColor = Color.red; // 色が変わった際の色

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isColorChanged = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isColorChanged)
        {
            StartCoroutine(ChangeColorTemporarily());
        }
        else if (other.gameObject.CompareTag("Enemy") && isColorChanged)
        {
            EnemyMove enemyMove = other.gameObject.GetComponent<EnemyMove>();
            if (enemyMove != null)
            {
                enemyMove.StopChasing(stopDuration);
            }
        }
    }

    private IEnumerator ChangeColorTemporarily()
    {
        isColorChanged = true;
        spriteRenderer.color = changedColor;

        yield return new WaitForSeconds(colorChangeDuration);

        spriteRenderer.color = originalColor;
        isColorChanged = false;
    }
}
