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
    public float rayHeightOffset = 1.0f; // �ˏo�_�̍����̃I�t�Z�b�g
    public string obstacleTag = "Obstacle"; // ��Q���̃^�O
    public Color rayColor = Color.red; // �f�o�b�O�p��Ray�̐F
    private Rigidbody2D rb;
    private Collider2D myCollider; // �������g��Collider
    private Quaternion initialRotation;
    public LayerMask StageLayer;
    public float knockbackForce = 500f; // ���˕Ԃ�̗�
    public float stopDuration = 1.0f; // ��~����
    public Camera mainCamera;  // ���C���J�����ւ̎Q��
    public float maxDistanceFromCamera = 10.0f; // �J��������̍ő勗��
    public Vector3 warpOffset = new Vector3(0, -10, 0); // �L�b�`���̏��ւ̃I�t�Z�b�g�ʒu
    public float jumpPower = 35.0f; // �W�����v�̗�

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>(); // ������Collider���擾
        initialRotation = gameObject.transform.rotation;
    }

    void FixedUpdate()
    {
        CheckDistanceFromCamera();

        if (isStopped)
        {
            anim.SetBool("run", false); // ��~���̓A�j���[�V�������~
            return; // ��~���͉������Ȃ�
        }

        // Ray�̕���������
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Raycast�ŏ�Q�����m
        if (!DetectObstacle(direction))
        {
            // �v���C���[-�G�L�����̈ʒu�֌W����������擾���A���x����艻
            Vector2 targeting = (player.transform.position - this.transform.position).normalized;

            if (targeting.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
                anim.SetBool("run", true); // �A�j���[�V�������Đ�
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
                anim.SetBool("run", true); // �A�j���[�V�������Đ�
            }
            // x�����ɂ̂݃v���C���[��ǂ�
            rb.velocity = new Vector2((targeting.x * speed), rb.velocity.y);
        }
        else
        {
            // ��Q�����������ꍇ�̏���
            MoveJump();
        }

        // Ray�������i�f�o�b�O�p�j
       Vector2 rayOrigin = (Vector2)transform.position + direction * 5.5f + new Vector2(0, rayHeightOffset); 
    Debug.DrawRay(rayOrigin, direction * rayDistance, rayColor); // �I�t�Z�b�g�����ʒu����Ray��`��
    }

    // �J��������̋������`�F�b�N����֐�
    private void CheckDistanceFromCamera()
    {
        float distanceFromCamera = Vector2.Distance(transform.position, mainCamera.transform.position);

        // �J���������苗�����ꂽ�烏�[�v����
        if (distanceFromCamera > maxDistanceFromCamera)
        {
            WarpToPlayer();
        }
    }

    // �v���C���[�̈ʒu�Ƀ��[�v���A�L�b�`���̉�����W�����v���鏈��
    private void WarpToPlayer()
    {
        // �v���C���[�̈ʒu�Ƀ��[�v���A�L�b�`���̏��ɐݒ�
        transform.position = player.transform.position + warpOffset;

        // ��莞�ԑ҂��Ă���W�����v
        StartCoroutine(JumpAfterDelay(2.0f)); // 0.5�b�ҋ@
    }

    // �W�����v���s���R���[�`��
    private IEnumerator JumpAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay); // �w�肳�ꂽ���ԑ҂�
                                                // �R���C�_�[�𖳌��ɂ���
        myCollider.enabled = false;

        // �W�����v���ăX�e�[�W�ɖ߂�
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);

        // �W�����v���n�܂�����A�����҂��Ă���R���C�_�[���ēx�L���ɂ���
        yield return new WaitForSeconds(0.5f); // �W�����v�̂��߂̎��Ԃ�҂i�K�v�ɉ����Ē����j

        // �R���C�_�[��L���ɂ���
        myCollider.enabled = true;
    }

    // Raycast�ŏ�Q�������m����֐��i�^�O�Ō��m�j
    private bool DetectObstacle(Vector2 direction)
    {
        // Ray�̎n�_�������O�ɃI�t�Z�b�g���A����������
        Vector2 rayOrigin = (Vector2)transform.position + direction * 5.5f + new Vector2(0, rayHeightOffset);

        // Ray�𔭎˂��đO���̏�Q�������m
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, rayDistance);

        // �������g��Ray��������Ȃ��悤�ɂ���
        if (hit.collider != null && hit.collider != myCollider)
        {
            if (hit.collider.CompareTag(obstacleTag))
            {
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
        // �ړ����~�߂�
        isStopped = true;
        rb.velocity = Vector2.zero; // ���x�����Z�b�g���Ē�~����
        anim.SetBool("run", false); // ��~���̓A�j���[�V��������~

        // ��~���鎞�Ԃ�҂�
        yield return new WaitForSeconds(duration);

        // �Ăѓ�����悤�ɂ���
        isStopped = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            stageCtrl.OnEnemyCollected();
        }

        // Trap�^�O�̃I�u�W�F�N�g�ɂԂ������ꍇ�̏���
        if (collision.gameObject.CompareTag("Trap"))
        {
            // �΂ߌ��ɒ��˕Ԃ鏈��
            Vector2 knockbackDirection = transform.localScale.x > 0 ? new Vector2(-1, 1) : new Vector2(1, 1); // ���E�Ə�����ɒ��˕Ԃ�
            rb.AddForce(knockbackDirection.normalized * knockbackForce); // AddForce�ŗ͂������Ē��˕Ԃ�

            // �ꎞ��~
            StopChasing(stopDuration);
        }
    }

    private void MoveJump()
    {
        if (GroundChk())
        {
            float jumpPower = 35.0f;
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }

    // �n�ʐڒn���m�֐�
    bool GroundChk()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position - new Vector3(0, 5.0f, 0); // 1���j�b�g���̈ʒu���I�_�Ƃ���

        gameObject.transform.rotation = initialRotation;
        Debug.DrawLine(startPosition, endPosition, Color.red);
        return Physics2D.Linecast(startPosition, endPosition, StageLayer);
    }
}
