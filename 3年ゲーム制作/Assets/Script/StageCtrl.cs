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
        Debug.Log("�`�[�Y���擾����܂����I");
        gameOverObj.SetActive(true);
        doGameOver = true;
    }

    public void Retry()
    {
        ChangeScene(1); //�ŏ��̃X�e�[�W�ɖ߂�̂łP
        retryGame = true;
    }

    public void ChangeScene(int num)
    {
        
            nextStageNum = num;
            
           
        
    }

}
