using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globe : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasFallen = false;

    [Header("プレイヤーの判定")]
    public PlayerTriggerCheck playerCheck;

    [Header("落ちてくるまでのラグ")]
    public float fallDelay = 0f;

    [Header("初動に加える力")]
    public Vector2 initialForce = new Vector2(-1f, -1f);
    public float forceMultiplier = 10f;

    [Header("転がる音の設定")]
    public AudioClip rollingSound; // 転がる音
    private AudioSource audioSource; // AudioSourceを管理

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;

        // AudioSourceの初期化
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = rollingSound;
        audioSource.loop = true; // ループ再生を有効化
        audioSource.volume = 0.5f; // 必要に応じて調整
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.isOn && !hasFallen)
        {
            hasFallen = true;
            StartCoroutine(FallAfterDelay());
        }
    }

    private IEnumerator FallAfterDelay()
    {
        if (fallDelay > 0f)
        {
            yield return new WaitForSeconds(fallDelay);
        }

        rb.constraints = RigidbodyConstraints2D.None;
        rb.AddForce(initialForce.normalized * forceMultiplier, ForceMode2D.Impulse);
        Debug.Log("Rigidbodyの制約解除");

        // 音を再生開始
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    // ぶつかったときの処理を追加
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // EnemyタグまたはPlayerタグを持つオブジェクトと衝突した場合
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            DisableSelfColliders();
            // 音をフェードアウト
            if (audioSource != null && audioSource.isPlaying)
            {
                StartCoroutine(FadeOutAndStopAudio(1f)); // フェードアウト時間を1秒に設定
            }
        }
    }

    // 自分自身のコライダーを無効化
    private void DisableSelfColliders()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        Debug.Log("自身のコライダーを無効化しました");
    }

    // 音をフェードアウトさせる
    private IEnumerator FadeOutAndStopAudio(float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // 次回再生に備えて音量を元に戻す
    }
}