using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 8f;
    public float dushSpeed = 10f;
    public LayerMask StageLayer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveRight();
        MoveJump();
    }

    //左右移動関数
    private void MoveRight()
    {
        transform.Translate(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0, 0);
    }

    //ジャンプ関数
    private void MoveJump()
    {
        if (GroundChk())
        {
            // ジャンプ操作
            if (Input.GetKeyDown(KeyCode.Space))
            {// ジャンプ開始
             // ジャンプ力を計算
                float jumpPower = 10.0f;
                // ジャンプ力を適用
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, jumpPower);
            }
        }  
    }

    //地面接地検知関数
    bool GroundChk()
    {
        Vector3 startposition = transform.position;                     // Playerの中心を始点とする
        Vector3 endposition = transform.position - transform.up; // Playerの足元を終点とする

        // Debug用に始点と終点を表示する
        Debug.DrawLine(startposition, endposition, Color.red);

        // Physics2D.Linecastを使い、ベクトルとStageLayerが接触していたらTrueを返す
        return Physics2D.Linecast(startposition, endposition, StageLayer);
    }
}
