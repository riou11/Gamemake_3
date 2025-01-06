using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatEnvironmentInteraction : MonoBehaviour
{
    public float knockbackForce = 500f;
    public float stopDuration = 1.0f;
    private Rigidbody2D rb;
    private CatState catState;
    public StageCtrl stageCtrl;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        catState = GetComponent<CatState>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Vector2 knockbackDirection = transform.localScale.x > 0 ? new Vector2(-1, 1) : new Vector2(1, 1);
            rb.AddForce(knockbackDirection.normalized * knockbackForce);

            StartCoroutine(StopMovementTemporarily(stopDuration));
            stageCtrl.OnCatDamage();
        }
    }

    private IEnumerator StopMovementTemporarily(float duration)
    {
        catState.SetStopped(true);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(duration);
        catState.SetStopped(false);
    }
}
