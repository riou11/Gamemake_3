using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMove : MonoBehaviour
{
    public GameObject player;
    public int speed;
    private bool isStopped = false;
    public StageCtrl stageCtrl; // StageCtrl�ւ̎Q��
    private Animator anim = null;
    public float rayDistance = 1.0f; // Ray�̋���
    public string obstacleTag = "Obstacle"; // ��Q���̃^�O
    public Color rayColor = Color.red; // �f�o�b�O�p��Ray�̐F
    private Rigidbody2D rb;
    private Collider2D myCollider; // �������g��Collider

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>(); // ������Collider���擾
    }

    void FixedUpdate()
    {
        // Ray�̕���������
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Raycast�ŏ�Q�����m
        if (!DetectObstacle(direction))
        {
            if (!isStopped)
            {
                // �v���C���[-�G�L�����̈ʒu�֌W����������擾���A���x����艻
                Vector2 targeting = (player.transform.position - this.transform.position).normalized;

                if (targeting.x > 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                    anim.SetBool("run", true);
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    anim.SetBool("run", true);
                }
                // x�����ɂ̂݃v���C���[��ǂ�
                rb.velocity = new Vector2((targeting.x * speed), rb.velocity.y);
            }
        }
        else
        {
            // ��Q�����������ꍇ�̏����������ɒǉ�
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("run", false);
        }

        // Ray�������i�f�o�b�O�p�j
        Vector2 rayOrigin = (Vector2)transform.position + direction * 5.5f; // �n�_�������O�ɃI�t�Z�b�g
        Debug.DrawRay(rayOrigin, direction * rayDistance, rayColor); // �I�t�Z�b�g�����ʒu����Ray��`��
    }

    // Raycast�ŏ�Q�������m����֐��i�^�O�Ō��m�j
    private bool DetectObstacle(Vector2 direction)
    {
        // Ray�̎n�_�������O�ɃI�t�Z�b�g
        Vector2 rayOrigin = (Vector2)transform.position + direction * 5.5f;

        // Ray�𔭎˂��đO���̏�Q�������m
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, rayDistance);

        // �������g��Ray��������Ȃ��悤�ɂ���
        if (hit.collider != null && hit.collider != myCollider)
        {
            Debug.Log("Raycast hit object: " + hit.collider.gameObject.name + " with tag: " + hit.collider.tag);

            // ��Q���̃^�O���m�F
            if (hit.collider.CompareTag(obstacleTag))
            {
                Debug.Log("Obstacle detected by tag!");
                return true;
            }
        }
        if (hit.collider != null)
        {
            Debug.Log("Raycast hit object: " + hit.collider.gameObject.name + " with tag: " + hit.collider.tag);
            if (hit.collider.CompareTag(obstacleTag))
            {
                Debug.Log("Obstacle detected by tag!");
                return true;
            }
        }

        return false;
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
