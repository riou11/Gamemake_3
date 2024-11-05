using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlootFloor : MonoBehaviour
{

    public float dropSpeed = 1.0f; // オブジェクトが下がる速度
    private bool isDropping = false; // オブジェクトが下がっているかどうかのフラグ

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤーに触れた瞬間にオブジェクトが下がり始める
        if (collision.gameObject.CompareTag("Player") && !isDropping)
        {
            isDropping = true;
            StartCoroutine(DropObject());
        }
    }

    private IEnumerator DropObject()
    {
        while (isDropping)
        {
            // オブジェクトを下方向に移動させる
            transform.position += Vector3.down * dropSpeed * Time.deltaTime;
            yield return null;
        }
    }
}