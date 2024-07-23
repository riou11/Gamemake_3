using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageCtrl : MonoBehaviour
{
    [Header("�v���C���[�Q�[���I�u�W�F�N�g")]
    public GameObject playerObj;
    [Header("�Q�[���I�[�o�[")]
    public GameObject gameOverObj;

    private bool doGameOver = false;
    private bool retryGame = false;
    private int nextStageNum;

    // Start is called before the first frame update
    void Start()
    {
        if (playerObj != null && gameOverObj != null)
        {
            gameOverObj.SetActive(false);
        }
        else
        {
            Debug.Log("�ݒ肪����ĂȂ���I");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doGameOver)
        {
            // �Q�[���I�[�o�[���̏����������ɒǉ�
        }
    }

    public void OnCheeseCollected()
    {
        Debug.Log("���e�`�[�Y���擾����܂����I");
        gameOverObj.SetActive(true);
        doGameOver = true;
    }

    public void OnEnemyCollected()
    {
        Debug.Log("�G�ƃv���C���[���ڐG���܂����I");
        gameOverObj.SetActive(true);
        doGameOver = true;
    }
    public void Retry0()
    {
        retryGame = true;
        ChangeScene(0); //�ŏ��̃X�e�[�W�ɖ߂�̂�1
    }
    public void Retry1()
    {
        retryGame = true;
        ChangeScene(1); //�ŏ��̃X�e�[�W�ɖ߂�̂�1
    }

    public void ChangeScene(int num)
    {
        nextStageNum = num;
        SceneManager.LoadScene(nextStageNum); // �V�[����ύX����
    }
}
