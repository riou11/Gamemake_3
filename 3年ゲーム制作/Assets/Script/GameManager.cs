using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class StageData
{
    public int stageNum;
    public bool isUnlocked;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public List<StageData> stages;

    //private bool retryGame = false;
    private int nextStageNum;

    PlayerMove player;

    //�X�e�[�WUI�؂�ւ�����̏���
    StageCtrl stageCtrl;

    //�`�[�Y�擾��
    private int cheeseScore = 0;

    //�e�X�e�[�W�̃`�[�Y�����
    private int[] cheeseScores = { 8, 0, 0 };

    //firstStage�̑��x�ꗗ
    private float[] firstStgPlySpeeds = { 6f, 7f, 8f, 8.5f, 9f, 9.5f, 10f, 10.5f, 11f, 11.5f };

    //���݂̃v���C���[���x�ۊǗp
    private float currentSpeed = 0f;

    //���݂̃X�e�[�W�ԍ�
    private int stgNum = 0;

    private float normalRunning = 0.1f;
    private float speedRunning = 0.3f;

    //�`�[�Y�擾��
    public float percentCheese { get; private set; }

    private bool IsStageCtrlGet = false;

    //�V���O���g���̎���
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //�Z�b�g�A�b�v
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if ((stageCtrl == null) || (player == null)) 
        {
            SetUp();
        }

        if (stageCtrl != null)
        {
            if (!stageCtrl.doGameOver)
            {
                if (IsStageCtrlGet)
                {
                    //�`�[�Y�Q�[�W�̍X�V
                    UpdateCheeseParameter();

                    //�`�[�Y�ۗL���ɂ��v���C���[���x�̍X�V
                    UpdateCurrentSpeed(stgNum);

                    //�v���C���[�̗͎̑���̍X�V
                    ManageHealth();
                }
                else
                {
                    StageCtrlSetUp();
                }
            }
            else
            {

            }
        } 
    }

    //�Z�b�g�A�b�v�֐�
    void SetUp()
    {
        Time.timeScale = 1.0f;

        player = FindObjectOfType<PlayerMove>();
        stageCtrl = FindObjectOfType<StageCtrl>();

        cheeseScore = 0;     

        //���݂̃X�e�[�W�ԍ��ɂ���āA�����x��ς��Ă���B
        switch (stgNum)
        {
            //firstStage
            case 0:
                currentSpeed = firstStgPlySpeeds[0];
                break;
        }

        if (stageCtrl != null)
        {
            StageCtrlSetUp();
        }
    }

    //�O����StageCtrl�N���X�̏�����(Find�֐��Ŏ擾����܂ŁA�s��Ȃ��悤�ɂ���)
    void StageCtrlSetUp()
    {
        stageCtrl.cheeseParameters.fillAmount = 0;
        stageCtrl.healthGaugeSlider.value = 1;
        IsStageCtrlGet = true;
    }

    void UpdateCheeseParameter()
    {
        //�l���`�[�Y���ƃX�e�[�W�ɔz�u���ꂽ�`�[�Y�������Z�������ʂ��A�`�[�Y�p�����[�^�[�ɔ��f
        percentCheese = (float)cheeseScore / (float)cheeseScores[stgNum];
        stageCtrl.cheeseParameters.fillAmount = percentCheese;
        Debug.Log(cheeseScore);
        Debug.Log(percentCheese);
    }

    //���x�̍X�V(�ϐ��͌��݂̃X�e�[�W�ԍ�)
    void UpdateCurrentSpeed(int stageNum)
    {
        //�v���C���[�̑��x���A�`�[�Y�̊l�����icheeseScore�ŊǗ��j����ύX
        switch (stageNum)
        {
            case 0:
                currentSpeed = firstStgPlySpeeds[cheeseScore];
                break;
        }
    }

    //PlayerMove�ɁA���݂̑��x��Ԃ�
    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    //�v���C���[�������Ă�����A�̗̓Q�[�W�����炷�B0�ɂȂ�����A�`�[�Y�����炷
    void ManageHealth()
    {
        if (player != null)
        {
            if (player.IsRunningPlayer())
            {
                if (cheeseScore != 0)
                {
                    if (player.IsPlayerDushing())
                    {
                        stageCtrl.healthGaugeSlider.value -= Time.deltaTime * speedRunning;
                    }
                    else
                    {
                        stageCtrl.healthGaugeSlider.value -= Time.deltaTime * normalRunning;
                    }
                }
            }
        }
        else
        {

        }

        if (stageCtrl.healthGaugeSlider.value <= 0)
        {
            //�`�[�Y�̎擾�����f�N�������g���A����̃X�s�[�h�ɕς���
            cheeseScore--;
            //�̗̓Q�[�W�����Z�b�g
            stageCtrl.healthGaugeSlider.value = 1;
        }
    }

    // �X�e�[�W�f�[�^���Z�[�u����
    public void SaveStageData()
    {
        // �f�[�^���V���A���C�Y���ĕۑ�
        PlayerPrefs.SetString("StageData", JsonUtility.ToJson(this));
    }

    // �X�e�[�W�f�[�^�����[�h����
    public void LoadStageData()
    {
        if (PlayerPrefs.HasKey("StageData"))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("StageData"), this);
        }
        else
        {
            // ����N�����ɍŏ��̃X�e�[�W�̂݉��
            stages[0].isUnlocked = true;
        }
    }

    // �X�e�[�W�N���A���Ɏ��̃X�e�[�W���A�����b�N
    public void UnlockNextStage(int clearedStageNumber)
    {
        if (clearedStageNumber < stages.Count - 1)
        {
            stages[clearedStageNumber + 1].isUnlocked = true;
            SaveStageData();
        }
    }

    public void StageCleared(int stageNumber)
    {
        UnlockNextStage(stageNumber);
        SceneManager.LoadScene("StageSelect");
    }

    public void getCheese(int cheese)
    {
        cheeseScore += cheese;
        //�V�����`�[�Y���Q�b�g������A�̗̓Q�[�W�����Z�b�g
        stageCtrl.healthGaugeSlider.value = 1;
    }
}
