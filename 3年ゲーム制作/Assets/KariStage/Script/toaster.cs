using System.Collections;
using UnityEngine;

public class toaster : MonoBehaviour
{
    [Header("動く距離、時間、待つ時間")]
    [SerializeField] private float distance = 1.0f;
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private float waitBeforeMove = 2.0f;
    [SerializeField] private string newTag = "Trap"; // 新しいタグ
    [Header("プレイヤーの判定")]
    [SerializeField] private string playerTag = "Player"; // プレイヤーのタグ
    [SerializeField] private AudioClip SE = null; // パンが上がるタイミングで鳴らすSE

    private AudioSource audioSource;
    private Collider2D col;
    private bool isMoved = false; // 動作済みフラグ

    void Start()
    {
        // AudioSourceを取得または追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        col = GetComponent<Collider2D>(); // 自分のコライダー取得
    }

    public void ActivateToaster()
    {
        if (!isMoved) // 一度しか動作しない
        {
            isMoved = true;
            StartCoroutine(Move());
        }
    }

    private IEnumerator Move()
    {
        // 下に移動
        Vector2 startPosition = transform.position;
        Vector2 downPosition = startPosition - new Vector2(0, distance / 2);
        Vector2 upPosition = startPosition + new Vector2(0, distance);

        float elapsedTime = 0;
        float downDuration = moveDuration / 2;

        while (elapsedTime < downDuration)
        {
            transform.position = Vector2.Lerp(startPosition, downPosition, elapsedTime / downDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = downPosition;

        // 待機時間
        yield return new WaitForSeconds(waitBeforeMove);

        // 上に移動開始
        elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(downPosition, upPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = upPosition;

        // 上に移動した後にSEを鳴らす
        PlaySE();

        // タグ変更
        gameObject.tag = newTag;
    }

    private void PlaySE()
    {
        if (SE != null)
        {
            audioSource.PlayOneShot(SE); // SEを鳴らす
        }
        else
        {
            Debug.LogWarning("パン上がるSEが設定されていません！");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(DisableColliderAndFadeOut());
        }
    }

    private IEnumerator DisableColliderAndFadeOut()
    {
        yield return new WaitForSeconds(0.5f); // 少し待つ
        col.enabled = false; // コライダー無効化
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float fadeDuration = 1.0f; // フェードアウトの時間
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 完全にフェードアウト後に非アクティブ化
        gameObject.SetActive(false);
    }
}
