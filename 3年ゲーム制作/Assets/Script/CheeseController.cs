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
    private bool isHeld = true;
    private bool isTouchingPlayer = false;

    public bool IsHeld { get { return isHeld; } }
    public int lives = 3;//チーズの残機

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
        // チーズが画面外に出たら消す処理
        if (transform.position.x < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x)
        {
            LoseCheese();
        }
    }

    void HoldCheese()
    {
        transform.position = cheeseHolder.transform.position;
        rb.isKinematic = true;
        isHeld = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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

    private void LoseCheese()
    {

        lives--;
        if(lives>0)
        {
            Invoke("HoldNewCheese", 0.5f);
        }
        else
        {
            Debug.Log("Game Over");
            //ゲームオーバー処理入れるならここに
        }
        Destroy(gameObject);
    }

    private void HoldNewCheese()
    {
        Debug.Log("New Cheese");
        //新しいチーズを持つ処理
        GameObject newCheese = Instantiate(gameObject, cheeseHolder.transform.position, Quaternion.identity);
        newCheese.GetComponent<CheeseController>().player = player;
        newCheese.GetComponent<CheeseController>().cheeseHolder = cheeseHolder;
        newCheese.GetComponent<CheeseController>().lives = lives;
    }
}

