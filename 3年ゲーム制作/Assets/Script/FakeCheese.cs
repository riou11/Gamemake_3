using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCheese : MonoBehaviour
{
    public GameObject player; // �v���C���[�I�u�W�F�N�g
    public GameObject cheeseHolder; // ���GameObject�i�v���C���[�̎q�I�u�W�F�N�g�j
    public float launchForce = 10f;
    public float upwardForce = 2f;
    private Rigidbody2D rb;
    private bool isHeld = false; // �ŏ��̓`�[�Y�������Ȃ�
    private bool isTouchingPlayer = false;
    public StageCtrl stageCtrl; // StageCtrl�ւ̎Q��

    public bool IsHeld { get { return isHeld; } }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            stageCtrl.OnCheeseCollected(); // �`�[�Y���擾���ꂽ���Ƃ�ʒm
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
        Debug.Log("���e�`�[�Y���v���C���[�ɐG��܂���");
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
