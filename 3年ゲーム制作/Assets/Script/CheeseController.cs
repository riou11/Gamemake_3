using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseController : MonoBehaviour
{
    public GameObject player; // プレイヤーオブジェクト
    public GameObject cheeseHolder; // 空のGameObject（プレイヤーの子オブジェクト）
    public float forwardForce = 10f;//前方向の力
    public float upwardForce = 2f;//上方向の力
    public float respawnDistance = 5f;
    private Rigidbody2D rb;
    private bool isHeld = true;
    private bool isTouchingPlayer = false;
    private CheeseManager cheeseManager;

    private Vector2 playerLastPosition;
    private Vector2 playerVelocity;
    public bool IsHeld { get { return isHeld; } }


    public void Initialize(GameObject player,GameObject cheeseHolder,CheeseManager cheeseManager)
    {
        this.player = player;
        this.cheeseHolder = cheeseHolder;
        this.cheeseManager= cheeseManager;
        rb=GetComponent<Rigidbody2D>();
        HoldCheese();
        playerLastPosition=player.transform.position;
    }

    void Update()
    {
        // プレイヤーの速度を計算
        playerVelocity = ((Vector2)player.transform.position - playerLastPosition) / Time.deltaTime;
        playerLastPosition = player.transform.position;
        if (isHeld)
        {
            transform.position = cheeseHolder.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Z) && isHeld)
        {
            isHeld = false;
            rb.isKinematic = false;


            Vector2 forwardDirection = new Vector2(player.transform.right.x, 0).normalized;
            Vector2 upwardDirection = new Vector2(0, 1).normalized;

            rb.AddForce(forwardDirection * forwardForce + upwardDirection * upwardForce, ForceMode2D.Impulse);
            rb.AddForce(playerVelocity, ForceMode2D.Impulse);//プレイヤーの速度を追加


        }
        if (isTouchingPlayer && !isHeld && Input.GetKeyDown(KeyCode.Z))
        {
            HoldCheese();
        }
        // チーズが画面外に出たら通知
        if (transform.position.x < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x)
        {
            cheeseManager.LoseCheese();
        }
        if(Vector2.Distance(player.transform.position,transform.position)>respawnDistance&&Input.GetKeyDown(KeyCode.Z))
        {
            cheeseManager.LoseCheese();
        }
    }

    void HoldCheese()
    {
        if (cheeseHolder != null)
        {
            transform.position = cheeseHolder.transform.position;
            rb.isKinematic = true;
            isHeld = true;
        }
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

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.transform.position, respawnDistance);
        }
    }
}

