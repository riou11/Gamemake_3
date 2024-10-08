using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toaster : MonoBehaviour
{
    [Header("���������A���ԁA�҂���")]
    [SerializeField] private float distance = 1.0f;
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private float waitBeforeMove = 2.0f;
    [Header("�v���C���[�̔���")] public PlayerTriggerCheck playerCheck;
    [SerializeField] AudioClip SE = null;
    AudioSource audioSource;

    private bool isMoved=false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCheck.isOn&&!isMoved)
        {
            isMoved = true;
            StartCoroutine(Move());
        }
    }

    public void ActivateTrap()
    {
        Debug.Log("Gimmick activated");
    }

    public void MoveObject()//�{�^���g���Ȃ炱��ō쓮
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

        // ���Ɉړ�����i�Z�����ԂŁj
        float downDuration = moveDuration / 2; // �����̎��Ԃŉ��Ɉړ�
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

        // ��Ɉړ�����
        elapsedTime = 0; // ���ԃ��Z�b�g
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(downPosition, upPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �ŏI�ʒu�ɓ��B
        transform.position = upPosition;
    }
}