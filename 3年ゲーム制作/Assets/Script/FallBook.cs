using UnityEngine;

public class FallBook : MonoBehaviour
{
    [SerializeField] private Transform player; // プレイヤーの Transform
    [SerializeField] private float triggerDistance = 5.0f; // オブジェクトが落下を開始する距離
    [SerializeField] private float fallSpeed = 5.0f; // 落下速度
    [SerializeField] private float stopYPosition = -10.0f; // オブジェクトが止まる Y 座標
    [SerializeField] private string groundTag = "Enemy"; // 地面のタグ

    private bool isFalling = false; // 落下中かどうかのフラグ
    private bool hasTriggeredCollision = false; // 衝突イベントを一度だけ呼び出すためのフラグ
    private PlayerMove playerScript; // プレイヤーのスクリプト参照

    void Start()
    {
        // プレイヤーオブジェクトのスクリプトを取得
        if (player != null)
        {
            playerScript = player.GetComponent<PlayerMove>();
        }

        if (playerScript == null)
        {
            Debug.LogError("PlayerMove スクリプトが見つかりませんでした。プレイヤーの設定を確認してください。");
        }
    }

    void Update()
    {
        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 一定距離内に入ったら落下を開始
        if (distanceToPlayer <= triggerDistance && !isFalling)
        {
            isFalling = true;
        }

        // 落下処理
        if (isFalling)
        {
            Fall();
        }
    }

    private void Fall()
    {
        // 落下中の位置を更新
        float newYPosition = transform.position.y - fallSpeed * Time.deltaTime;

        // 新しい位置が停止位置を下回る場合、停止位置に固定
        if (newYPosition <= stopYPosition)
        {
            transform.position = new Vector3(transform.position.x, stopYPosition, transform.position.z);
            isFalling = false; // 落下を終了

            // タグを「Untagged」に変更
            gameObject.tag = "Untagged";
        }
        else
        {
            transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーと衝突した場合の処理
        if (collision.CompareTag("Player") && !hasTriggeredCollision)
        {
            hasTriggeredCollision = true; // 衝突処理が一度だけ実行されるようにフラグを設定

            // プレイヤーの OnEnemyCollision を呼び出し
            if (playerScript != null)
            {
                StartCoroutine(playerScript.OnEnemyCollision());
            }
        }

        // 地面と衝突した場合、タグを変更
        if (collision.CompareTag(groundTag))
        {
            gameObject.tag = "Untagged";
        }
    }
}
