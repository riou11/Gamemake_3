using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : MonoBehaviour
{
    public GameObject player;
    public int speed;
    private bool isStopped = false;
    public StageCtrl stageCtrl; // StageCtrlへの参照
    private Animator anim = null;
    public float rayDistance = 1.0f; // Rayの距離
    public string obstacleTag = "Obstacle"; // 障害物のタグ
    public Color rayColor = Color.red; // デバッグ用のRayの色
    private Rigidbody2D rb;
    private Collider2D myCollider; // 自分自身のCollider
    private Quaternion initialRotation;
    public LayerMask StageLayer;
    public float knockbackForce = 500f; // 跳ね返りの力
    public float stopDuration = 1.0f; // 停止時間

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>(); // 自分のColliderを取得
        initialRotation = gameObject.transform.rotation;
    }

    void FixedUpdate()
    {
        if (isStopped)
        {
            anim.SetBool("run", false); // 停止中はアニメーションを停止
            return; // 停止中は何もしない
        }

        // Rayの方向を決定
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Raycastで障害物検知
        if (!DetectObstacle(direction))
        {
            // プレイヤー-敵キャラの位置関係から方向を取得し、速度を一定化
            Vector2 targeting = (player.transform.position - this.transform.position).normalized;

            if (targeting.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                anim.SetBool("run", true); // アニメーションを再生
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
                anim.SetBool("run", true); // アニメーションを再生
            }
            // x方向にのみプレイヤーを追う
            rb.velocity = new Vector2((targeting.x * speed), rb.velocity.y);
        }
        else
        {
            // 障害物があった場合の処理
            MoveJump();
        }

        // Rayを可視化（デバッグ用）
        Vector2 rayOrigin = (Vector2)transform.position + direction * 5.5f; // 始点を少し前にオフセット
        Debug.DrawRay(rayOrigin, direction * rayDistance, rayColor); // オフセットした位置からRayを描画
    }

    // Raycastで障害物を検知する関数（タグで検知）
    private bool DetectObstacle(Vector2 direction)
    {
        // Rayの始点を少し前にオフセット
        Vector2 rayOrigin = (Vector2)transform.position + direction * 5.5f;

        // Rayを発射して前方の障害物を検知
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, rayDistance);

        // 自分自身にRayが当たらないようにする
        if (hit.collider != null && hit.collider != myCollider)
        {
            if (hit.collider.CompareTag(obstacleTag))
            {
                return true;
            }
        }

        return false;
    }

    public void StopChasing(float duration)
    {
        StartCoroutine(StopChasingCoroutine(duration));
    }

    private IEnumerator StopChasingCoroutine(float duration)
    {
        // 移動を止める
        isStopped = true;
        rb.velocity = Vector2.zero; // 速度をリセットして停止する
        anim.SetBool("run", false); // 停止中はアニメーションも停止

        // 停止する時間を待つ
        yield return new WaitForSeconds(duration);

        // 再び動けるようにする
        isStopped = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            stageCtrl.OnEnemyCollected();
        }

        // Trapタグのオブジェクトにぶつかった場合の処理
        if (collision.gameObject.CompareTag("Trap"))
        {
            // 斜め後ろに跳ね返る処理
            Vector2 knockbackDirection = transform.localScale.x > 0 ? new Vector2(-1, 1) : new Vector2(1, 1); // 左右と上方向に跳ね返る
            rb.AddForce(knockbackDirection.normalized * knockbackForce); // AddForceで力を加えて跳ね返す

            // 一時停止
            StopChasing(stopDuration);
        }
    }

    private void MoveJump()
    {
        if (GroundChk())
        {
            float jumpPower = 35.0f;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    // 地面接地検知関数
    bool GroundChk()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - new Vector3(0, 5.0f, 0); // 1ユニット下の位置を終点とする

        gameObject.transform.rotation = initialRotation;
        Debug.DrawLine(startPosition, endPosition, Color.red);
        return Physics2D.Linecast(startPosition, endPosition, StageLayer);
    }
}
