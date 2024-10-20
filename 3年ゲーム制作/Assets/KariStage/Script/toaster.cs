using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toaster : MonoBehaviour
{
    [Header("���������A���ԁA�҂���")]
    [SerializeField] private float distance = 1.0f;
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private float waitBeforeMove = 2.0f;
    [SerializeField] private string newTag = "Trap"; // �V�����^�O��Inspector����ݒ�ł���
    [Header("�v���C���[�̔���")] public PlayerTriggerCheck playerCheck;
    [SerializeField] AudioClip SE = null;
    AudioSource audioSource;

    private bool isMoved = false;
    private Collider2D col;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>(); // �����̃R���C�_�[�擾
    }

    void Update()
    {
        if (playerCheck.isOn && !isMoved)
        {
            isMoved = true;
            StartCoroutine(Move());
        }
    }

    public void ActivateTrap()
    {
        Debug.Log("Gimmick activated");
    }

    public void MoveObject() // �{�^���g���Ȃ炱��ō쓮
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        // �������Ɉړ�
        Vector2 startPosition = transform.position;
        Vector2 downPosition = startPosition - new Vector2(0, distance / 2); // ���ɏ����ړ�����
        Vector2 upPosition = startPosition + new Vector2(0, distance); // �ŏI�I�ɏ�Ɉړ�����ʒu

        float elapsedTime = 0;
        float downDuration = moveDuration / 2; // �����̎��Ԃŉ��Ɉړ�

        // ���Ɉړ�����
        while (elapsedTime < downDuration)
        {
            transform.position = Vector2.Lerp(startPosition, downPosition, elapsedTime / downDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �ŏI�ʒu�ɓ��B
        transform.position = downPosition;

        // �w��b���ҋ@
        yield return new WaitForSeconds(waitBeforeMove);

        // ��Ɉړ��J�n�i�����Ń^�O��ύX�j
        gameObject.tag = newTag; // �^�O�ύX
        elapsedTime = 0;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(downPosition, upPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = upPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // �L�ɓ��������Ƃ��Ɉ�莞�Ԍ�ɃR���C�_�[�������^�O�ύX
            StartCoroutine(DisableColliderAndChangeTagAfterDelay());
        }
    }

    private IEnumerator DisableColliderAndChangeTagAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // �����҂�
        col.enabled = false; // �R���C�_�[����
        gameObject.tag = "Untagged"; // �^�O�����Z�b�g
        StartCoroutine(FadeOut()); // �����Ȃ����鏈��
    }

    private IEnumerator FadeOut()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float fadeDuration = 1.0f;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���S�Ɍ����Ȃ��Ȃ�����I�u�W�F�N�g���A�N�e�B�u��
        gameObject.SetActive(false);
    }
}
