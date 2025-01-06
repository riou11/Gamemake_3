using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatJumpAndObstacle : MonoBehaviour
{
    public float rayDistance = 1.0f;
    public float rayHeightOffset = 1.0f;
    public string obstacleTag = "Obstacle";
    public Color rayColor = Color.red;
    public LayerMask StageLayer;
    public float jumpPower = 35.0f;

    private Rigidbody2D rb;
    private Collider2D myCollider;
    private Quaternion initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        initialRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        if (!DetectObstacle(direction))
        {
            return;
        }

        if (GroundChk())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    private bool DetectObstacle(Vector2 direction)
    {
        Vector2 rayOrigin = (Vector2)transform.position + direction * 0.5f + new Vector2(0, rayHeightOffset);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, rayDistance);

        if (hit.collider != null && hit.collider.CompareTag(obstacleTag))
        {
            return true;
        }

        return false;
    }

    private bool GroundChk()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - new Vector3(0, 5.0f, 0);
        Debug.DrawLine(startPosition, endPosition, Color.red);
        return Physics2D.Linecast(startPosition, endPosition, StageLayer);
    }
}
