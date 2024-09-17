using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class PlayerMove : MonoBehaviour
{

    private Animator anim = null;

    //enumでネズミの状態をStateとして管理
    public enum State
    {
        normal,
        catchRope,
        releaseRope
    }

    public State state;

    public float speed = 8f;
    public float dushSpeed = 1.5f;
    private float currentSpeed = 0f; //現在の速度
    private Quaternion initialRotation;
    public LayerMask StageLayer;

    private Rigidbody2D rb;

    void Start()
    {
        state = State.normal;
        initialRotation = gameObject.transform.rotation;
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
            // catchRope状態の処理
        }
        else
        {
            // その他の状態の処理
        }


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


    }

    //左右移動関数
    private void MoveRight()
    {
        float horizontalKey = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = speed * dushSpeed;
        }
        else
        {
            currentSpeed = speed;
        }

        transform.Translate(Input.GetAxisRaw("Horizontal") * currentSpeed * Time.deltaTime, 0, 0);
    }

    //ジャンプ関数
    private void MoveJump()
    {
        if (GroundChk())
        {
            // ジャンプ操作
            if (Input.GetKeyDown(KeyCode.Space))
            {// ジャンプ開始
             // ジャンプ力を計算
                float jumpPower = 10.0f;
                // ジャンプ力を適用
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
        }
    }

    //地面接地検知関数
    bool GroundChk()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - new Vector3(0, 2.0f, 0); // 1ユニット下の位置を終点とする

        gameObject.transform.rotation = initialRotation;

        // Debug用に始点と終点を表示する
        Debug.DrawLine(startPosition, endPosition, Color.red);

        // Physics2D.Linecastを使い、ベクトルとStageLayerが接触していたらTrueを返す
        return Physics2D.Linecast(startPosition, endPosition, StageLayer);

        
    }
}
