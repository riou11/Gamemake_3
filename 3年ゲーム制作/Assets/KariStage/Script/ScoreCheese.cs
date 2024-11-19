using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class ScoreCheese : MonoBehaviour
{
    [SerializeField] AudioClip getSE=null;
    [Header("加算するスコア")]public int myScore;
    [Header("プレイヤーの判定")]public PlayerTriggerCheck playerCheck;

    GameManager gameManager;
    AudioSource audioSource;
    bool isCollected = false; // 取得済み判定用
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // 自動再生を無効化
        audioSource.clip = getSE; // SEを設定
    }

    // Update is called once per frame
    void Update()
    {
        // 既に取得済みなら何もしない
        if (isCollected) return;

        if (playerCheck.isOn)
        {
            // 取得判定を無効化
            isCollected = true;

            // GameManager側にスコア加算
            gameManager.getCheese(1);

            // 見た目を消す（Renderer無効化）
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }

            // コライダーを無効化
            foreach (Collider collider in GetComponents<Collider>())
            {
                collider.enabled = false;
            }

            // SEを再生（AudioClipが設定されている場合のみ）
            if (getSE != null)
            {
                audioSource.Play();
                // SE再生終了後に削除
                StartCoroutine(DestroyAfterSE());
            }
            else
            {
                // SEがない場合はすぐ削除
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator DestroyAfterSE()
    {
        // SEの再生時間待機
        yield return new WaitForSeconds(getSE.length);

        // オブジェクトを削除
        Destroy(this.gameObject);
    }
}
