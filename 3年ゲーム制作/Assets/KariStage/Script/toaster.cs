using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toaster : MonoBehaviour
{
    [Header("動く距離、時間、待つ時間")]
    [SerializeField] private float distance = 1.0f;
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private float waitBeforeMove = 2.0f;
    [Header("プレイヤーの判定")] public PlayerTriggerCheck playerCheck;
    [SerializeField] AudioClip SE = null;
    AudioSource audioSource;

    private bool isMoved=false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.isOn&&!isMoved)
        {
            isMoved = true;
            StartCoroutine(Move());
        }
    }

    public void ActivateTrap()
    {
        Debug.Log("Gimmick activated");
    }

    public void MoveObject()//ボタン使うならこれで作動
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

        // 下に移動する（短い時間で）
        float downDuration = moveDuration / 2; // 半分の時間で下に移動
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

        // 上に移動する
        elapsedTime = 0; // 時間リセット
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(downPosition, upPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 最終位置に到達
        transform.position = upPosition;
    }
}