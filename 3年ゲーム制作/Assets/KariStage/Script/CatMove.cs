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

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>(); // 自分のColliderを取得
        initialRotation = gameObject.transform.rotation;
    }

    void FixedUpdate()
    {
        // Rayの方向を決定
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Raycastで障害物検知
        if (!DetectObstacle(direction))
        {
            if (!isStopped)
            {
                // プレイヤー-敵キャラの位置関係から方向を取得し、速度を一定化
                Vector2 targeting = (player.transform.position - this.transform.position).normalized;

                if (targeting.x > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                    anim.SetBool("run", true);
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    anim.SetBool("run", true);
                }
                // x方向にのみプレイヤーを追う
                rb.velocity = new Vector2((targeting.x * speed), rb.velocity.y);
            }
        }
        else
        {
            // 障害物があった場合の処理をここに追加
            //rb.velocity = new Vector2(0, rb.velocity.y);
            //anim.SetBool("run", false);
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
            //Debug.Log("Raycast hit object: " + hit.collider.gameObject.name + " with tag: " + hit.collider.tag);

            // 障害物のタグを確認
            if (hit.collider.CompareTag(obstacleTag))
            {
                //Debug.Log("Obstacle detected by tag!");
                return true;
            }
        }
        if (hit.collider != null)
        {
            //Debug.Log("Raycast hit object: " + hit.collider.gameObject.name + " with tag: " + hit.collider.tag);
            if (hit.collider.CompareTag(obstacleTag))
            {
                //Debug.Log("Obstacle detected by tag!");
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
        isStopped = true;
        yield return new WaitForSeconds(duration);
        isStopped = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            stageCtrl.OnEnemyCollected();
        }
    }
    private void MoveJump()
    {
        if (GroundChk())
        {

            float jumpPower = 10.0f;
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
        bool hoge = Physics2D.Linecast(startPosition, endPosition, StageLayer);
        Debug.Log(hoge);
        return Physics2D.Linecast(startPosition, endPosition, StageLayer);
    }
}
