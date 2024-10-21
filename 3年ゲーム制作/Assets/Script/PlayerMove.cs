using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    GameManager gameManager;

    private HingeJoint2D hingeJoint;
    private bool isAttached = false;
    private Animator anim = null;
    private float speed = 0f;
    private float dushSpeed = 1.5f;
    private float currentSpeed = 0f;
    private Quaternion initialRotation;
    public LayerMask StageLayer;
    private Rigidbody2D rb;
    
    private bool IsDushing = false;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        initialRotation = gameObject.transform.rotation;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        hingeJoint = GetComponent<HingeJoint2D>();
        hingeJoint.enabled = false; // 初期は無効
    }

    void Update()
    {
        // プレイヤーがロープに掴まっていない場合のみ移動とジャンプを許可
        if (!isAttached && gameManager != null) 
        {
            MoveRight();
            MoveJump();
        }

        if (isAttached && Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            anim.SetBool("hook", false);
            ReleaseRope(); // ロープを離す

        }

        if (isAttached)
        {

            anim.SetBool("hook", true);
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

        if (GroundChk())
        {
            anim.ResetTrigger("Jump"); // 地面に着いたらジャンプをリセット
            anim.SetBool("Jumping", false); // 空中状態のフラグを解除
        }
        else
        {
            anim.SetBool("Jumping", true); // 空中状態にフラグを立てる
        }
    }

    //プレイヤーが走っているかを、アニメーションのStateから判断し、真偽を返す
    public bool IsRunningPlayer()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Player_run"))
        {
            return true;
        }

        return false;
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
        hingeJoint.connectedBody = null;
        isAttached = false;
    }

    // 左右移動関数
    private void MoveRight()
    {
        float horizontalKey = Input.GetAxis("Horizontal");

        //GameManagerから返ってきた、currentSpeedを適用
        speed = gameManager.GetCurrentSpeed();
        Debug.Log(speed);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = speed * dushSpeed;
            IsDushing = true;
        }
        else
        {
            currentSpeed = speed;
            IsDushing = false;
        }

        transform.Translate(Input.GetAxisRaw("Horizontal") * currentSpeed * Time.deltaTime, 0, 0);
    }

    public bool IsPlayerDushing()
    {
        return IsDushing;
    }

    // ジャンプ関数
    private void MoveJump()
    {
        if (GroundChk() && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))) 
        {
            float jumpPower = 10.0f;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            anim.SetTrigger("Jump");
        }
    }

    // 地面接地検知関数
    bool GroundChk()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - new Vector3(0, 2.0f, 0);

        gameObject.transform.rotation = initialRotation;
        Debug.DrawLine(startPosition, endPosition, Color.red);

        return Physics2D.Linecast(startPosition, endPosition, StageLayer);
    }
}