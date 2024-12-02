using System.Collections;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [Header("レバーの動作設定")]
    [SerializeField] private float leverMoveDistance = 0.2f; // レバーが下がる距離
    [SerializeField] private float leverMoveDuration = 0.3f; // 下がるのにかかる時間
    [SerializeField] private float waitBeforeLeverReset = 1.0f; // レバーが戻る前の待機時間
    [SerializeField] private string playerTag = "Player"; // プレイヤーのタグ

    [Header("連動するトースター")]
    [SerializeField] private toaster toasterScript; // トースターのスクリプト

    //[Header("レバーのSE設定")]
    //[SerializeField] private AudioClip leverMoveSE; // レバーを下げたときのSE
    //private AudioSource audioSource;

    private Vector2 originalPosition; // レバーの元の位置
    private bool isLeverActivated = false; // レバーが既に動作済みか

    void Start()
    {
        originalPosition = transform.position; // 元の位置を記録

        //// AudioSourceを取得または追加
        //audioSource = GetComponent<AudioSource>();
        //if (audioSource == null)
        //{
        //    audioSource = gameObject.AddComponent<AudioSource>();
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag) && !isLeverActivated)
        {
            isLeverActivated = true; // 動作フラグを立てる
            StartCoroutine(ActivateLever());
        }
    }

    private IEnumerator ActivateLever()
    {
        // レバーが下がる動作
        Vector2 downPosition = originalPosition - new Vector2(0, leverMoveDistance);
        float elapsedTime = 0;

        // SEを鳴らす
        //PlayLeverMoveSE();

        while (elapsedTime < leverMoveDuration)
        {
            transform.position = Vector2.Lerp(originalPosition, downPosition, elapsedTime / leverMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = downPosition;

        // レバーが下がり切ったらパンを作動させる
        toasterScript.ActivateToaster();

        // レバーが戻る前に待機
        yield return new WaitForSeconds(waitBeforeLeverReset);

        // レバーが元に戻る動作
        elapsedTime = 0;

        while (elapsedTime < leverMoveDuration)
        {
            transform.position = Vector2.Lerp(downPosition, originalPosition, elapsedTime / leverMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }

    //private void PlayLeverMoveSE()
    //{
    //    if (leverMoveSE != null)
    //    {
    //        audioSource.PlayOneShot(leverMoveSE);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("レバーのSEが設定されていません！");
    //    }
    //}
}
