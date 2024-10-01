using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GameObject player;
    public int speed;
    void FixedUpdate()
    {
        // �v���C���[-�G�L�����̈ʒu�֌W����������擾���A���x����艻
        Vector2 targeting = (player.transform.position - this.transform.position).normalized;
        if (targeting.x > 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
        // x�����ɂ̂݃v���C���[��ǂ�
        this.GetComponent<Rigidbody2D>().velocity = new Vector2((targeting.x * speed), 0);
    }
}
