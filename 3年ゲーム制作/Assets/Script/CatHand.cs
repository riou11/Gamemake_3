using System.Collections;
using UnityEngine;

public class CatHand : MonoBehaviour
{
    [SerializeField] private float moveDistance = 2.0f; // 上下の移動距離
    [SerializeField] private float moveSpeed = 2.0f;    // 移動速度
    [SerializeField] private float pauseDuration = 1.0f; // 停止時間

    private Vector3 startPosition;
    private bool isMovingUp = true; // 現在の移動方向を追跡

    void Start()
    {
        startPosition = transform.position;
        StartCoroutine(MoveWithPause());
    }

    private IEnumerator MoveWithPause()
    {
        while (true)
        {
            // 目的地を計算
            Vector3 targetPosition = isMovingUp
                ? startPosition + new Vector3(0, moveDistance, 0)
                : startPosition - new Vector3(0, moveDistance, 0);

            // 目的地に向かって移動
            while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null; // フレームを待機
            }

            // 停止時間を待つ
            yield return new WaitForSeconds(pauseDuration);

            // 移動方向を切り替え
            isMovingUp = !isMovingUp;
        }
    }
}
