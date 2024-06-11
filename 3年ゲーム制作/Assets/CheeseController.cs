using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseController : MonoBehaviour
{
    public GameObject player; // �v���C���[�I�u�W�F�N�g
    public GameObject cheeseHolder; // ���GameObject�i�v���C���[�̎q�I�u�W�F�N�g�j
    public float launchForce = 10f;
    public float upwardForce = 2f;
    private Rigidbody2D rb;
    private bool isHeld = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HoldCheese();
    }

    void Update()
    {
        if (isHeld)
        {
            transform.position = cheeseHolder.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isHeld)
        {
            isHeld = false;
            rb.isKinematic = false;
            Vector2 launchDirection = new Vector2(player.transform.right.x, player.transform.right.y + upwardForce).normalized;
            rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
        }
    }

    void HoldCheese()
    {
        transform.position = cheeseHolder.transform.position;
        rb.isKinematic = true;
        isHeld = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player && !isHeld)
        {
            // Cheese���v���C���[�ɐG�ꂽ�猳�̈ʒu�ɖ߂�
            HoldCheese();
        }
    }
}

