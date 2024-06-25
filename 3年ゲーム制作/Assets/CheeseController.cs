using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseController : MonoBehaviour
{
    public GameObject player; // プレイヤーオブジェクト
    public GameObject cheeseHolder; // 空のGameObject（プレイヤーの子オブジェクト）
    public float launchForce = 10f;
    public float upwardForce = 2f;
    private Rigidbody2D rb;
    private bool isHeld = true;//持ってる状態か
    private bool isTouchingPlayer = false;//Playerに触れているか


    public bool IsHeld { get { return isHeld; } }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        HoldCheese();
    }

    void Update()
    {
        if (isHeld)
        {
            transform.position = cheeseHolder.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Z) && isHeld)
        {
            isHeld = false;
            rb.isKinematic = false;
            Vector2 launchDirection = new Vector2(player.transform.right.x, player.transform.right.y + upwardForce).normalized;
            rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
        }
        if (isTouchingPlayer && !isHeld && Input.GetKeyDown(KeyCode.Z))
        {
            HoldCheese();
        }
    }

    void HoldCheese()
    {
        transform.position = cheeseHolder.transform.position;
        rb.isKinematic = true;
        Debug.Log("Hold");
        isHeld = true;
        isTouchingPlayer = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hureteru");
        if (collision.gameObject == player && !isHeld)
        {
            isTouchingPlayer = true; // プレイヤーに触れている状態を記録
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            isTouchingPlayer = false; // プレイヤーから離れた状態を記録
        }
    }
}

