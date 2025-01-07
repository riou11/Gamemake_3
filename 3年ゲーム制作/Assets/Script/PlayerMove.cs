using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    GameManager gameManager => GameManager.Instance;
    [SerializeField] StageCtrl stageCtrl;
    //public GameObject Enemy;
    private bool isAttached = false;
    private bool isControllable = true; // 操作可能かどうかのフラグ
    private Animator anim = null;
    private float speed = 0f;
    private float currentSpeed = 0f;
    private Quaternion initialRotation;
    public LayerMask StageLayer;
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private Transform attachedObject; // 固定するオブジェクトの位置を保持

    void Start()
    {
        //gameManager = FindObjectOfType<GameManager>();
        initialRotation = gameObject.transform.rotation;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>(); // プレイヤーのコライダーを取得
        isControllable = true;
    }

    void Update()
    {
        // プレイヤーが固定されていない場合のみ移動とジャンプを許可
        if (isControllable && !isAttached && gameManager != null)
        {
            MoveRight();
            MoveJump();
        }

        // Spaceキーで解放とジャンプ
        if (isAttached && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")))
        {
            SoundManager.Instance.PlaySFX(SoundManager.SoundType.jump);
            anim.SetBool("hook", false);
            ReleaseObject(); // 固定を解除
        }

        if (isAttached)
        {
            anim.SetBool("hook", true);
            // 固定中のオブジェクトの位置にプレイヤーを固定
            if (attachedObject != null)
                transform.position = attachedObject.position;
        }

        // アニメーション処理（固定されていない場合のみ）
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

    // 衝突処理で特定タグのオブジェクトに接触した際の挙動
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rope") && !isAttached)
        {
            AttachToObject(collision.gameObject.transform);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(OnEnemyCollision());
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            stageCtrl.arrivedGoal();
        }
    }


    public IEnumerator OnEnemyCollision()//死んだ時のアニメーション処理
    {
        SoundManager.Instance.PlaySFX(SoundManager.SoundType.Dead_nezumi);
        isControllable = false; // 操作不可
        anim.SetBool("Death", true); // デスアニメーションを再生

        // 少し待ってから上に打ち上げる
        yield return new WaitForSeconds(0.5f);
        rb.velocity = new Vector2(0, 30f); // 上方向に力を加える

        // 当たり判定を無効化
        playerCollider.enabled = false;

        // 必要に応じて一定時間後にリセット
        yield return new WaitForSeconds(1.0f); // アニメーション再生後の待機時間
        anim.SetBool("Death", false); // アニメーションを停止
        // isControllable = true;
        stageCtrl.OnEnemyCollected();

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

    void AttachToObject(Transform objTransform)
    {
        attachedObject = objTransform;
        isAttached = true;
    }

    void ReleaseObject()
    {
        isAttached = false;

        // オブジェクトから少し離れた位置に移動して衝突を避ける
        if (attachedObject != null)
        {
            Vector2 offset = new Vector2(0, 0.5f); // 少し上に離す
            transform.position = (Vector2)attachedObject.position + offset;
            attachedObject = null;
        }

        // ジャンプ前に速度を完全にリセット
        rb.velocity = Vector2.zero;

        // プレイヤーの入力方向を取得
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // ジャンプ力を一定にする
        float jumpPower = 22.5f;
        float horizontalForce = 5.0f; // 水平方向の力（一定値）

        // 新しい力を加える
        rb.AddForce(new Vector2(horizontalInput * horizontalForce, jumpPower), ForceMode2D.Impulse);

        // アニメーションをトリガー
        anim.SetTrigger("Jump");
    }

    // 左右移動関数
    private void MoveRight()
    {
        float horizontalKey = Input.GetAxis("Horizontal");
        //GameManagerから返ってきた、currentSpeedを適用
        speed = stageCtrl.GetCurrentSpeed();
        Debug.Log(speed);

        currentSpeed = speed;

        transform.Translate(Input.GetAxisRaw("Horizontal") * currentSpeed * Time.deltaTime, 0, 0);
    }

    // ジャンプ関数
    private void MoveJump()
    {
        if (GroundChk() && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump")))
        {
            SoundManager.Instance.PlaySFX(SoundManager.SoundType.jump);
            float jumpPower = 22.5f;
            rb.velocity = new Vector2(0, jumpPower);
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
