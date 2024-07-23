using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public GameObject player;
    public int speed;
    private bool isStopped = false;
    public StageCtrl stageCtrl; // StageCtrl�ւ̎Q��

    void FixedUpdate()
    {
        if (!isStopped)
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
        else
        {
            // ��~���͑��x���[���ɂ���
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void StopChasing(float duration)
    {
        StartCoroutine(StopChasingCoroutine(duration));
    }

    private IEnumerator StopChasingCoroutine(float duration)
    {
        isStopped = true;
        yield return new WaitForSeconds(duration);
        isStopped = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            stageCtrl.OnEnemyCollected();
        }
    }
}
