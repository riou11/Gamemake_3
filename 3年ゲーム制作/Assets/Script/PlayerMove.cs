using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayerMove : MonoBehaviour
{
    //enum�Ńl�Y�~�̏�Ԃ�State�Ƃ��ĊǗ�
    public enum State
    {
        normal,
        catchRope,
        releaseRope
    }

    public State state;

    public float speed = 8f;
    public float dushSpeed = 1.5f;
    private float currentSpeed = 0f; //���݂̑��x
    private Animator anim = null;
    public LayerMask StageLayer;

    private Rigidbody2D rb;

    void Start()
    {
        state = State.normal;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.normal)
        {
            MoveRight();
            MoveJump();
        }
        else if (state == State.catchRope)
        {
            // catchRope��Ԃ̏���
        }
        else
        {
            // ���̑��̏�Ԃ̏���
        }
    }

    //���E�ړ��֐�
    private void MoveRight()
    {
        float horizontalKey = Input.GetAxis("Horizontal");

        if (horizontalKey > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            anim.SetBool("run", true);
        }
        else if (horizontalKey < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            anim.SetBool("run", true);
        }
        else
        {
            anim.SetBool("run", false);
        }


        //X�{�^���������ꂽ�Ƃ��ɁA���x��ύX�i�_�b�V���j
        if (Input.GetKey(KeyCode.X))
        {
            currentSpeed = speed * dushSpeed;
        }
        else
        {
            currentSpeed = speed;
        }

        transform.Translate(Input.GetAxisRaw("Horizontal") * currentSpeed * Time.deltaTime, 0, 0);
    }

    //�W�����v�֐�
    private void MoveJump()
    {
        if (GroundChk())
        {
            // �W�����v����
            if (Input.GetKeyDown(KeyCode.Z))
            {
                // �W�����v�J�n
                float jumpPower = 10.0f;
                // �W�����v�͂�K�p
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
        }
    }

    //�n�ʐڒn���m�֐�
    bool GroundChk()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - new Vector3(0, 1.0f, 0); // 1���j�b�g���̈ʒu���I�_�Ƃ���

        // Debug�p�Ɏn�_�ƏI�_��\������
        Debug.DrawLine(startPosition, endPosition, Color.red);

        // Physics2D.Linecast���g���A�x�N�g����StageLayer���ڐG���Ă�����True��Ԃ�
        return Physics2D.Linecast(startPosition, endPosition, StageLayer);
    }
}
