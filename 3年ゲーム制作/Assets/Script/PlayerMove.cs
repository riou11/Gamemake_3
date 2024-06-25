using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 8f;
    public float dushSpeed = 10f;
    public LayerMask StageLayer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveRight();
        MoveJump();
    }

    //���E�ړ��֐�
    private void MoveRight()
    {
        transform.Translate(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0, 0);
    }

    //�W�����v�֐�
    private void MoveJump()
    {
        if (GroundChk())
        {
            // �W�����v����
            if (Input.GetKeyDown(KeyCode.Space))
            {// �W�����v�J�n
             // �W�����v�͂��v�Z
                float jumpPower = 10.0f;
                // �W�����v�͂�K�p
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, jumpPower);
            }
        }  
    }

    //�n�ʐڒn���m�֐�
    bool GroundChk()
    {
        Vector3 startposition = transform.position;                     // Player�̒��S���n�_�Ƃ���
        Vector3 endposition = transform.position - transform.up; // Player�̑������I�_�Ƃ���

        // Debug�p�Ɏn�_�ƏI�_��\������
        Debug.DrawLine(startposition, endposition, Color.red);

        // Physics2D.Linecast���g���A�x�N�g����StageLayer���ڐG���Ă�����True��Ԃ�
        return Physics2D.Linecast(startposition, endposition, StageLayer);
    }
}
