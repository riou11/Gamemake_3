using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toaster : MonoBehaviour
{
    [SerializeField] private float distance = 1.0f;
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private float waitBeforeMove = 2.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActivateTrap()
    {
        Debug.Log("Gimmick activated");
    }

    public void MoveObject()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        // 指定秒数待機
        yield return new WaitForSeconds(waitBeforeMove);

        Vector2 startPosition = transform.position;
        Vector2 endPosition = startPosition + new Vector2(0, distance);
        float elapsedTime = 0;

        // パンを指定時間かけて移動させる
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 最終位置に移動
        transform.position = endPosition;
    }
}