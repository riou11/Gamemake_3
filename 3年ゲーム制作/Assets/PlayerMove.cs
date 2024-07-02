using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayerMove : MonoBehaviour
{
    //enum�Ńl�Y�~�̏�Ԃ�State�Ƃ��ĊǗ�
    public enum State { 
        normal,
        catchRope,
        releaseRope
    }

    public State state;

    public float speed = 8f;
    public float dushSpeed = 1.5f; 
    private float currentSpeed = 0f; //���݂̑��x
    public LayerMask StageLayer;

    void Start()
    {
        state = State.normal;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.normal)
        {
            MoveRight();
            MoveJump();
        }
        else if(state == State.catchRope)
        {

        }
        else {

        }
    }

    //���E�ړ��֐�
    private void MoveRight()
    {
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
