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
        // Enemyタグを持つオブジェクトと衝突した場合
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            DisableColliders(collision.gameObject);
        }
        // 音を停止
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // 指定されたオブジェクトとその子オブジェクトのコライダーを無効化
    private void DisableColliders(GameObject obj)
    {
        Collider2D[] colliders = obj.GetComponentsInChildren<Collider2D>();

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        Debug.Log($"コライダーを無効化しました: {obj.name}");
    }
}