using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private HingeJoint2D hingeJoint;
    private bool isAttached = false;
    private Animator anim = null;
    public StageCtrl stageCtrl;

    public float speed = 8f;
    public float dushSpeed = 1.5f;
    private float currentSpeed = 0f;
    private Quaternion initialRotation;
    public LayerMask StageLayer;
    private Rigidbody2D rb;

    void Start()
    {
        initialRotation = gameObject.transform.rotation;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
<<<<<<< HEAD
        hingeJoint = gameObject.AddComponent<HingeJoint2D>();
        hingeJoint.enabled = false; // 初期は無効
=======
>>>>>>> develop
    }

    void Update()
    {
        // プレイヤーがロープに掴まっていない場合のみ移動とジャンプを許可
        if (!isAttached)
        {
            MoveRight();
            MoveJump();
        }

        if (isAttached && Input.GetKeyDown(KeyCode.Space))
        {
            ReleaseRope(); // ロープを離す
        }

        // アニメーション処理（ここもロープに掴まっていない場合のみ）
        if (!isAttached)
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
        }
    }


    // 衝突処理で紐に接触した際の挙動
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Rope" && !isAttached)
        {
            AttachToRope(collision.gameObject);
        }
    }

    void AttachToRope(GameObject rope)
    {
        hingeJoint.connectedBody = rope.GetComponent<Rigidbody2D>();
        hingeJoint.enabled = true;
        isAttached = true;
    }

    void ReleaseRope()
    {
        hingeJoint.enabled = false;
        isAttached = false;
    }

    // 左右移動関数
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

    // ジャンプ関数
    private void MoveJump()
    {
        if (GroundChk() && Input.GetKeyDown(KeyCode.C))
        {
<<<<<<< HEAD
            float jumpPower = 10.0f;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
=======
            // ジャンプ操作
            if (Input.GetKeyDown(KeyCode.Space))
            {// ジャンプ開始
             // ジャンプ力を計算
                float jumpPower = 7.0f;
                // ジャンプ力を適用
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
>>>>>>> develop
        }
    }

    // 地面接地検知関数
    bool GroundChk()
    {
        Vector3 startPosition = transform.position;
<<<<<<< HEAD
        Vector3 endPosition = transform.position - new Vector3(0, 2.0f, 0);
=======
        Vector3 endPosition = transform.position - new Vector3(0, 2.0f, 0); // 1ユニット下の位置を終点とする
>>>>>>> develop

        gameObject.transform.rotation = initialRotation;
        Debug.DrawLine(startPosition, endPosition, Color.red);

        return Physics2D.Linecast(startPosition, endPosition, StageLayer);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Goal")
        {
            stageCtrl.arrivedGoal();
        }
    }
}
