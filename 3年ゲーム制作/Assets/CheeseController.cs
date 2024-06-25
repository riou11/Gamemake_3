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
    private bool isHeld = true;//�����Ă��Ԃ�
    private bool isTouchingPlayer = false;//Player�ɐG��Ă��邩


    public bool IsHeld { get { return isHeld; } }

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

        if (Input.GetKeyDown(KeyCode.Z) && isHeld)
        {
            isHeld = false;
            rb.isKinematic = false;
            Vector2 launchDirection = new Vector2(player.transform.right.x, player.transform.right.y + upwardForce).normalized;
            rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
        }
        if (isTouchingPlayer && !isHeld && Input.GetKeyDown(KeyCode.Z))
        {
            HoldCheese();
        }
    }

    void HoldCheese()
    {
        transform.position = cheeseHolder.transform.position;
        rb.isKinematic = true;
        Debug.Log("Hold");
        isHeld = true;
        isTouchingPlayer = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hureteru");
        if (collision.gameObject == player && !isHeld)
        {
            isTouchingPlayer = true; // �v���C���[�ɐG��Ă����Ԃ��L�^
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            isTouchingPlayer = false; // �v���C���[���痣�ꂽ��Ԃ��L�^
        }
    }
}

