using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toaster : MonoBehaviour
{
    [Header("動く距離、時間、待つ時間")]
    [SerializeField] private float distance = 1.0f;
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private float waitBeforeMove = 2.0f;
    [SerializeField] private string newTag = "Trap"; // 新しいタグをInspectorから設定できる
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    [SerializeField] AudioClip SE = null;
    AudioSource audioSource;

    private bool isMoved = false;
    private Collider2D col;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>(); // 自分のコライダー取得
    }

    void Update()
    {
        if (playerCheck.isOn && !isMoved)
        {
            isMoved = true;
            StartCoroutine(Move());
        }
    }

    public void ActivateTrap()
    {
        Debug.Log("Gimmick activated");
    }

    public void MoveObject() // ボタン使うならこれで作動
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        // 少し下に移動
        Vector2 startPosition = transform.position;
        Vector2 downPosition = startPosition - new Vector2(0, distance / 2); // 下に少し移動する
        Vector2 upPosition = startPosition + new Vector2(0, distance); // 最終的に上に移動する位置

        float elapsedTime = 0;
        float downDuration = moveDuration / 2; // 半分の時間で下に移動

        // 下に移動する
        while (elapsedTime < downDuration)
        {
            transform.position = Vector2.Lerp(startPosition, downPosition, elapsedTime / downDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 最終位置に到達
        transform.position = downPosition;

        // 指定秒数待機
        yield return new WaitForSeconds(waitBeforeMove);

        // 上に移動開始（ここでタグを変更）
        gameObject.tag = newTag; // タグ変更
        elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(downPosition, upPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = upPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // 猫に当たったときに一定時間後にコライダー無効＆タグ変更
            StartCoroutine(DisableColliderAndChangeTagAfterDelay());
        }
    }

    private IEnumerator DisableColliderAndChangeTagAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // 少し待つ
        col.enabled = false; // コライダー無効
        gameObject.tag = "Untagged"; // タグもリセット
        StartCoroutine(FadeOut()); // 見えなくする処理
    }

    private IEnumerator FadeOut()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float fadeDuration = 1.0f;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 完全に見えなくなったらオブジェクトを非アクティブに
        gameObject.SetActive(false);
    }
}
